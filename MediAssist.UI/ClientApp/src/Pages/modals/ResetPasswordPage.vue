<template>
  <div class="reset-password-page">
    <div class="background-container">
      <iframe src="/" class="background-blur" title="Descriptive title here"></iframe>
    </div>

    <div v-if="!isValidToken" class="error-container">
      <img src="@/assets/error-image.gif" alt="Error" class="error-image"/>
      <p class="error-message">{{ validationMessage }}</p>
    </div>

    <div v-else class="reset-password-popup">
      <h2>Reset Password</h2>
      <form @submit.prevent="resetPassword">
          <div class="input-group">
              <label for="newPassword">Enter your Password</label>
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

        <p v-if="validationMessage" class="error-message">{{ validationMessage }}</p>
        <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
        <button type="submit" class="reset-button" :disabled="!isFormValid">Reset Password</button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import axios from 'axios';

const route = useRoute();
const router = useRouter();
const token = ref('');
const userEmailId = ref('');
const newPassword = ref('');
const confirmPassword = ref('');
const passwordVisible = ref(false);
const confirmPasswordVisible = ref(false);
const isPasswordInputActive = ref(false);
const isConfirmPasswordInputActive = ref(false);
const errors = ref({
  newPassword: '',
  confirmPassword: '',
});
const errorMessage  = ref('');
const validationMessage = ref('');
const isValidToken = ref(false);

onMounted(() => {
  token.value = route.query.token;
  userEmailId.value = route.query.email;
  validateToken();
});

const togglePasswordVisibility = () => {
  passwordVisible.value = !passwordVisible.value;
};

const toggleConfirmPasswordVisibility = () => {
  confirmPasswordVisible.value = !confirmPasswordVisible.value;
};

const interactWithField = (field) => {
  isPasswordInputActive.value = field === 'newPassword' && newPassword.value.length > 0;
  isConfirmPasswordInputActive.value = field === 'confirmPassword' && confirmPassword.value.length > 0;
};

const validateField = (field) => {
  if (field === 'newPassword') {
    validateNewPassword();
  } else if (field === 'confirmPassword') {
    validateConfirmPassword();
  }
};

const isFormValid = computed(() => {
  return newPassword.value && confirmPassword.value && !errors.value.newPassword && !errors.value.confirmPassword;
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
  } else {
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

const validateToken = async () => {
    try {
        const response = await axios.get('/api/manage/validate-reset-token', {
            params: { token: token.value }
        });

        isValidToken.value = response.data.success;

        if (isValidToken.value) {
            userEmailId.value = response.data.email;
            validationMessage.value = response.data.message;
        }else if(!isValidToken.value) {
            validationMessage.value = response.data.message;
        }
    } catch (error) {
        if (error.response && error.response.data && error.response.data.message) {
            validationMessage.value = error.response.data.message;
        } else {
            // Default error message if the API doesn't provide one
            validationMessage.value = 'Token validation failed.';
        }

        isValidToken.value = false;
    }
};

const resetPassword = async () => {
  validateField('newPassword');
  validateField('confirmPassword');

  if (isFormValid.value) {
    try {
      const response = await axios.post('/api/manage/reset-password', {
        Password: newPassword.value,
        ConfirmPassword: confirmPassword.value,
        Email: userEmailId.value,
        Token: token.value,
      });

      const data = response.data;

      if (data.success) {
        localStorage.clear();
        sessionStorage.clear();
        const clearCookie = (name) => {
        document.cookie = `${name}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT; SameSite=Strict`;
        };
        ['userId', 'firstName', 'specialization', 'title'].forEach(clearCookie);
        sessionStorage.setItem(
          'successMessage',
          'Your password has been reset successfully. Please sign in using your new password'
        );
        router.push('/login');
      } else {
        errorMessage.value = data.message;
      }
    } catch (error) {
      errorMessage.value = 'An error occurred while resetting your password. Please try again.';
    }
  } 
};



const checkPasswordInput = () => {
    interactWithField('newPassword');

    if (validationMessage.value) {
        validationMessage.value = '';
    }
};

const handleConfirmPasswordInput = () => {
    interactWithField('confirmPassword');
    if (errors.value.confirmPassword) {
        errors.value.confirmPassword = '';
    }
    if (validationMessage.value) {
        validationMessage.value = '';
    }
};
</script>

<style scoped>
.reset-password-page {
  position: relative;
  width: 100%;
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  overflow: hidden;
}

.background-container {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 1;
}

.background-blur {
    width: 100%;
    height: 100%;
    filter: blur(10px);
    pointer-events: none;
    border: none;
}

.error-container {
  position: relative;
  z-index: 2;
  background-color: white;
  padding: 30px;
  border-radius: 12px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  max-width: 400px;
  width: 100%;
  text-align: center;
}

.error-image {
  width: 200px;
  height: auto;
}

.error-message {
  color: red;
  font-size: 16px;
  margin-top: 10px;
}

.reset-password-popup {
  position: relative;
  z-index: 2;
  background-color: white;
  padding: 30px;
  border-radius: 12px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  max-width: 400px;
  width: 100%;
}

.input-group {
  margin-bottom: 20px;
}

.input-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
  color: #333;
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
}

.reset-button:disabled {
  background-color: #91d8f7; 
  cursor: not-allowed;
}

.error {
  color: red;
  font-size: 14px;
  margin-top: 5px;
}
</style>
