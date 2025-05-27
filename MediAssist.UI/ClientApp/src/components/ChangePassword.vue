<template>
  <div class="change-password-page">
    <div class="change-password-popup">
      <div class="modal-header">
        <button class="close-button" @click="cancel">
          <span>&times;</span>
        </button>
      </div>
      <h2 class="modal-title">Change Password</h2>
      <form @submit.prevent="changePassword">
          <div class="input-group">
              <label for="currentPassword">Current Password</label>
              <div class="password-wrapper">
            <input
               v-model="currentPassword"
                         :type="currentPasswordVisible ? 'text' : 'password'"
                         id="currentPassword"
                         @focus="interactWithField('currentPassword')"
                         @blur="checkPassword"
                         @input="checkCurrentPasswordInput"
              required
            />
                  <button v-if="isCurrentPasswordInputActive" type="button" @click="toggleCurrentPasswordVisibility">
                      <i :class="currentPasswordVisible ? 'pi pi-eye-slash' : 'pi pi-eye'"></i>
                  </button>
              </div>
              <span v-if="currentPasswordError" class="error">{{ currentPasswordError }}</span>
          </div>
          
          <div class="input-group">
              <label for="newPassword">New Password</label>
              <div class="password-wrapper">
            <input
               v-model="newPassword"
                         :type="passwordVisible ? 'text' : 'password'"
                         id="newPassword"
                         @focus="interactWithField('newPassword')"
                         @blur="validateField('newPassword')"
                         @input="checkPasswordInput"
              required
            />
                  <button v-if="isPasswordInputActive" type="button" @click="togglePasswordVisibility">
                      <i :class="passwordVisible ? 'pi pi-eye-slash' : 'pi pi-eye'"></i>
                  </button>
              </div>
              <span v-if="errors.newPassword" class="error">{{ errors.newPassword }}</span>
          </div>

          <div class="input-group">
              <label for="confirmPassword">Confirm Password</label>
              <div class="password-wrapper">
            <input
              v-model="confirmPassword"
                         :type="confirmPasswordVisible ? 'text' : 'password'"
                         id="confirmPassword"
                         @focus="interactWithField('confirmPassword')"
                         @blur="validateField('confirmPassword')"
                         @input="handleConfirmPasswordInput"
              required
            />
                  <button v-if="isConfirmPasswordInputActive" type="button" @click="toggleConfirmPasswordVisibility">
                      <i :class="confirmPasswordVisible ? 'pi pi-eye-slash' : 'pi pi-eye'"></i>
                  </button>
              </div>
              <span v-if="errors.confirmPassword" class="error">{{ errors.confirmPassword }}</span>
          </div>

        <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
        <button type="submit" class="reset-button" :disabled="!isFormValid">Change Password</button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed, defineEmits, watch } from 'vue';
import { useRouter } from 'vue-router';
import axiosInstance from '@/Services/Interceptors/axios.js';

const router = useRouter();
const newPassword = ref('');
const confirmPassword = ref('');
const currentPassword = ref('');
const passwordVisible = ref(false);
const confirmPasswordVisible = ref(false);
const currentPasswordVisible = ref(false);
const isPasswordInputActive = ref(false);
const isConfirmPasswordInputActive = ref(false);
const isCurrentPasswordInputActive = ref(false);
const errorMessage  = ref('');
const userId = ref('');
const currentPasswordError = ref('');

const emits = defineEmits(['close']);
const cancel = () => emits('close');
const errors = ref({
  newPassword: '',
  confirmPassword: '',
});

watch(currentPassword, () => {
  if (newPassword.value) {
    validateNewPassword();
  }
});

function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
  return null;
}

onMounted(() => {
  userId.value = getCookie("userId");
});

const toggleCurrentPasswordVisibility = () => {
  currentPasswordVisible.value = !currentPasswordVisible.value;
};
const togglePasswordVisibility = () => {
  passwordVisible.value = !passwordVisible.value;
};

const toggleConfirmPasswordVisibility = () => {
  confirmPasswordVisible.value = !confirmPasswordVisible.value;
};

const interactWithField = (field) => {
  isPasswordInputActive.value = field === 'newPassword' && newPassword.value.length > 0;
  isConfirmPasswordInputActive.value = field === 'confirmPassword' && confirmPassword.value.length > 0;
  isCurrentPasswordInputActive.value = field === 'currentPassword' && currentPassword.value.length > 0;
};

const validateField = (field) => {
  if (field === 'newPassword') {
    validateNewPassword();
  } else if (field === 'confirmPassword') {
    validateConfirmPassword();
  }
};

const isFormValid = computed(() => {
  return newPassword.value && confirmPassword.value && currentPassword.value && !errors.value.newPassword && !errors.value.confirmPassword;
});

const validateNewPassword = () => {
  const pwd = newPassword.value;
  if (!pwd) {
    errors.value.newPassword = 'Password is required.';
  } else if (pwd.length < 8) {
    errors.value.newPassword = 'Password must be at least 8 characters long.';
  } else if (!/[A-Z]/.test(pwd)) {
    errors.value.newPassword = 'Password must include at least one uppercase letter.';
  } else if (!/[a-z]/.test(pwd)) {
    errors.value.newPassword = 'Password must include at least one lowercase letter.';
  } else if (!/\d/.test(pwd)) {
    errors.value.newPassword = 'Password must include at least one digit.';
  } else if (!/[!@#$%^&*(),.?":{}|<>_+\-=\\`~[\]'/;]/.test(pwd)) {
    errors.value.newPassword = 'Password must include at least one special character.';
  } else if (pwd === currentPassword.value) {
    errors.value.newPassword = 'New password must be different from the current one.';
  }else {
    errors.value.newPassword = '';
  }
};

const validateConfirmPassword = () => {
  if (!confirmPassword.value) {
      errors.value.confirmPassword = 'Please confirm your password.';
  }
  else if (confirmPassword.value !== newPassword.value) {
      errors.value.confirmPassword = 'Passwords do not match. Please try again.';
      confirmPassword.value = '';
    } else {
    errors.value.confirmPassword = '';
  }
};

window.addEventListener('storage', (event) => {
  if (event.key === 'logout-event') {
    router.push('/login');
  }
});

const checkPassword = async () => {
  try {
    if (!currentPassword.value) {
      currentPasswordError.value = 'Current password is required.';
      return false;
    }else if (currentPassword.value.length < 8) {
      currentPasswordError.value = 'Please enter the correct password to proceed.';
      return false;
    }
    
    const response = await axiosInstance.get('/api/manage/passwordcheck', {
      params: {
        userId: userId.value,
        password: currentPassword.value
      }
    });
    const data = response.data;

    if (data.ifPasswordCorrect === false) {
      currentPasswordError.value = 'Please enter the correct password to proceed.';
      return false; 
    } else if (data.ifPasswordCorrect === true) {
      currentPasswordError.value = ''; 
      return true; 
    } 
  } catch (error) {
    errorMessage.value = 'An error occurred. Please try again.';
    return false;
  }
};
const changePassword = async () => {
  localStorage.setItem('logout-event', Date.now());
  validateField('newPassword');
  validateField('confirmPassword');
  if (isFormValid.value) {
    try {
      const response = await axiosInstance.post(`/api/manage/change-password`, {
        NewPassword: newPassword.value,
        ConfirmPassword: confirmPassword.value,
        Password: currentPassword.value,
        UserId: userId.value
      });

      const data = response.data;
      if (data.success) {
        await axiosInstance.post('/api/manage/logout');
        localStorage.clear();
        sessionStorage.clear();
        const clearCookie = (name) => {
      document.cookie = `${name}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT; SameSite=Strict`;
      };
     ['userId', 'firstName', 'specialization', 'title'].forEach(clearCookie);
        sessionStorage.setItem(
          'successMessage',
          'Your password has been successfully changed. Please sign in using your new password'
        );
         localStorage.setItem('userLoggedOut', Date.now());
        redirectAndLockOnLoginPage();
      } else {
        errorMessage.value = data.message;
      }
    } catch (error) {
      errorMessage.value = 'An error occurred while changing your password. Please try again.';
    }   
  }
};

function redirectAndLockOnLoginPage() {
  router.replace('/login');
  history.pushState(null, null, '/login');
  window.onpopstate = function () {
    history.pushState(null, null, '/login');
  };
}

window.addEventListener('storage', (event) => {
    if (event.key === 'userLoggedOut') {
      redirectAndLockOnLoginPage();
    }
});

const checkPasswordInput = () => {
  interactWithField('newPassword');
    errors.value.newPassword = '';
}

const checkCurrentPasswordInput = () => {
  interactWithField('currentPassword');
      currentPasswordError.value = '';
}
const handleConfirmPasswordInput = () => {
    interactWithField('confirmPassword');
        errors.value.confirmPassword = ''; 
};
</script>

<style scoped>
.change-password-page {
 position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: flex-start;
  z-index: 1000;
}

.close-button {
  width: 32px;
  height: 32px;
  padding: 8px;
  top: 6px;
  position: absolute;
  right: 20px;
  font-size: 35px;
  cursor: pointer;
  color: #aaa;
  border: none;
  background: none;
}
.close-button:hover {
  color: black;
}

.change-password-popup {
  position: relative;
  z-index: 2;
  background-color: white;
  padding: 40px 40px 40px 40px;
  border-radius: 12px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  max-width: 350px;
  width: 70%;
  top: 20%;
}

.input-group {
  margin-bottom: 20px;
  padding: 4px;
}

.input-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: 400;
  color: var(--Text-Text-Label, #6D6D6D);
  font-size: 16px;
  font-style: normal;
}

.input-group input {
  width: 100%; 
  padding: 10px 40px 10px 10px; 
  border-radius: 4px;
  border: 1px solid #ccc;
  font-size: 14px;
  box-sizing: border-box; 
}

.password-wrapper {
  position: relative;
}

.password-wrapper button {
  position: absolute;
  right: 10px;
  top: 50%;
  transform: translateY(-50%);
  background: transparent;
  border: none;
  cursor: pointer;
}

.reset-button {
  width: 100%;
  padding: 10px;
  border: none;
  border-radius: 4px;
  background-color: #06c;
  color: white;
  font-size: 16px;
  cursor: pointer;
  margin-top: 30px;
}

.reset-button:disabled {
  background-color: #91d8f7; 
  cursor: not-allowed;
}

.error {
  color: red;
  font-size: 13px;
  margin-top: 5px;
  position: absolute;
}
.error-message{
  color: red;
  font-size: 13px;
  margin-top: 5px;
}
.modal-title {
  width: 100%;
  height: auto;
  font-style: normal;
}
@media (max-width: 575px) { 
  .reset-button{
    margin-top: 15px;
  }
  .change-password-popup{
    padding: 25px;
  }
}
@media (min-width: 576px) and (max-width: 767px) {
  .reset-button{
    margin-top: 20px;
  }
  .change-password-popup{
    padding: 35px 35px 35px 35px;
  }
}

</style>
