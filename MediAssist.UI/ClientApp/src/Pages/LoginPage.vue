<template>
  <div class="website login-container">
      <div class="image-side">
          <img src="@/assets/loginPageImage.jpg" alt="login page" />
      </div>

      <div class="form-side">
          <header class="header-container">
              <div class="logo">
                  <AppLogo />
              </div>
              <p class="signup-option">
                  Don’t have an account?
                  <a href="/signup" @click="redirectToSignUp">SignUp</a>
              </p>
          </header>
          <div class="form-content">
              <div v-if="successMessage" class="success-message">{{ successMessage }}</div>
              <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
              <h2 class="form-heading">Login</h2>

              <form @submit.prevent="handleLogin">
                  <div class="input-group">
                      <label for="email">Email</label>
                      <input v-model="email"
                             type="text"
                             id="email"
                             @input="trimEmail" 
                             @paste="handlePaste"
                             @focus="interactWithField('email')"
                             @blur="validateField('email')"
                             required />
                      <span v-if="errors.email" class="error">{{ errors.email }}</span>
                  </div>

                  <div class="input-group">
                      <label for="password">Password</label>
                      <div class="password-wrapper">
                          <input
                              v-model="password"
                              :type="passwordVisible ? 'text' : 'password'"
                              id="password"
                              @focus="interactWithField('password')"
                              @blur="validateField('password')"
                              @input="checkPasswordInput()"
                              required
                          />
                          <button v-if="isPasswordInputActive" type="button" @click="togglePasswordVisibility">
                              <i :class="passwordVisible ? 'pi pi-eye-slash' : 'pi pi-eye'"></i>
                          </button>
                          <span v-if="errors.password" class="error">{{ errors.password }}</span>
                      </div>
                  </div>

                  <div class="options">
                      <label>
                          <input type="checkbox" v-model="rememberMe" />
                          Remember me
                      </label>
                      <a href="#" class="forgot-password" @click.prevent="showForgotPasswordModal = true">Forgot Password?</a>
                  </div>
                  <button type="submit" class="login-button" :disabled="!isFormValid || isLoading">
                      <span v-if="isLoading">
                          <i class="pi pi-spinner pi-spin"></i> Loading...
                      </span>
                      <span v-else>
                          Login
                      </span>
                  </button>
              </form>
          </div>
      </div>
      <ForgetPassword 
        :is-visible="showForgotPasswordModal" 
        title="Forgot Password"
        description="Enter your registered email address to receive a password reset link"
        submitButtonText="Submit"
        modal-type="forgot-password"
        @close="showForgotPasswordModal = false"
        />
  </div>
</template>

<script setup>
import AppLogo from '@/components/AppLogo.vue';
import { computed, onMounted, onUnmounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import ForgetPassword from './modals/GenericModal.vue';
import axios from 'axios';

// State variables
const router = useRouter();
const email = ref('');
const password = ref('');
const rememberMe = ref(false);
const passwordVisible = ref(false);
const isPasswordInputActive = ref(false);
const successMessage = ref('');
const errorMessage = ref('');
const showForgotPasswordModal = ref(false);
const errors = ref({ email: '', password: '' });
const interacted = ref({ email: false, password: false });
const isLoading = ref(false);

// Watch for field interaction
const interactWithField = (field) => {
  interacted.value[field] = true;
};

// Handle mounting/unmounting events
onMounted(() => {
  successMessage.value = sessionStorage.getItem('successMessage') || '';
  sessionStorage.removeItem('successMessage');
  errorMessage.value = sessionStorage.getItem('errorMessage') || '';
  sessionStorage.removeItem('errorMessage');
  document.addEventListener('click', clearMessage);
});

onUnmounted(() => {
  document.removeEventListener('click', clearMessage);
});

// Helper functions
const togglePasswordVisibility = () => {
  passwordVisible.value = !passwordVisible.value;
};

const checkPasswordInput = () => {
  isPasswordInputActive.value = password.value.trim().length > 0;
};

const clearMessage = () => {
  successMessage.value = '';
  sessionStorage.removeItem('successMessage');
  errorMessage.value = '';
};

const trimEmail = () => {
  email.value = email.value.trim();
};

const handlePaste = () => {
  setTimeout(() => (email.value = email.value.trim()), 0);
};

// Validation logic
const validateField = (field) => {
  if (!interacted.value[field]) return;

  if (field === 'email') {
    const trimmedEmail = email.value.trim();
    const emailPattern = /^[a-zA-Z0-9][\w.-]*[a-zA-Z0-9]@[a-zA-Z0-9]+[\w.-]*[a-zA-Z0-9]\.[a-zA-Z]{2,}$/;

    if (!trimmedEmail) {
      errors.value.email = 'Email is required.';
    } else if (!emailPattern.test(trimmedEmail)) {
      errors.value.email = 'Enter a valid email address.';
    } else {
      errors.value.email = '';
    }
    email.value = trimmedEmail;
  }

  if (field === 'password') {
    if (!password.value) {
      errors.value.password = 'Password is required.';
    } else if (password.value.length < 8) {
      errors.value.password = 'Password must be at least 8 characters long.';
    } else {
      errors.value.password = '';
    }
  }
};

// Form validation status
const isFormValid = computed(() => {
  return email.value && password.value && !errors.value.email && !errors.value.password;
});

// Login logic
const handleLogin = async () => {
  validateField('email');
  validateField('password');

  if (errors.value.email || errors.value.password) return;

  isLoading.value = true;
  try {
    const response = await axios.post('/api/manage/login', {
    Email: email.value,
    Password: password.value,
    RememberMe: rememberMe.value,
    }, {
      headers: {
        'Content-Type': 'application/json',
      },
    });
    const data = response.data;

    if (response.status === 200 && data.success) {
      localStorage.setItem('authToken', data.authToken);
      sessionStorage.removeItem('logoutState');
      window.onpopstate = null;
      const expirationDate = new Date(data.expirationTime).toUTCString();
      document.cookie = `userId=${data.userId}; path=/; Secure; SameSite=Strict; expires=${expirationDate}`;
      document.cookie = `firstName=${data.firstName}; path=/; Secure; SameSite=Strict; expires=${expirationDate}`;
      document.cookie = `specialization=${data.specialization}; path=/; Secure; SameSite=Strict; expires=${expirationDate}`;
      document.cookie = `title=${data.title}; path=/; Secure; SameSite=Strict; expires=${expirationDate}`;
      document.cookie = `email=${email.value}; path=/; Secure; SameSite=Strict; expires=${expirationDate}`;
      localStorage.setItem('image', data.image);
      await router.push(data.redirectUrl);

      if (!data.isSettingsUpdated){
        setTimeout(() => {
          window.dispatchEvent(
            new CustomEvent("update-settings", {
              detail: {
                success: true,
                message: "Complete your settings page to access all features and benefits",
              },
            })
          );
        }, 100);
      }
    } else {
      errorMessage.value = data.message;
    }
  } catch (error) {
    if(error.response.data.message){
      errorMessage.value = error.response.data.message;
    }
    else{
    errorMessage.value = 'An error occurred. Please try again.';
    }
  } finally {
    isLoading.value = false;
  }
};

// Navigation
const redirectToSignUp = () => {
  router.push('/signup');
};
</script>



<style scoped>
  @import url('https://fonts.googleapis.com/css2?family=Karma:wght@400;500&display=swap');

  .error {
      color: red;
      font-size: 12px;
  }

  .login-container {
      display: flex;
      height: 100vh;
  }

  .success-message {
      font-size: 14px;
      margin-bottom: 10px;
      color: green;
  }

  .error-message {
      font-size: 14px;
      margin-bottom: 10px;
      color: red;
  }

  .image-side {
      flex: 1;
      position: relative;
      display: flex;
      align-items: center;
      justify-content: center;
      overflow: hidden;
  }

      .image-side img {
          width: 100%;
          height: 100%;
          object-fit: cover;
      }

  .form-side {
      flex: 1;
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: center;
      padding: 20px;
      position: relative;
      background-color: white;
  }

  .header-container {
      display: flex;
      justify-content: space-between;
      align-items: center;
      width: 100%;
      max-width: 600px;
      padding: 20px;
      box-sizing: border-box;
  }

  .logo {
      display: flex;
      align-items: center;
      cursor: pointer;
      margin-right: 20px;
      text-decoration: none;
  }

      .logo h3 {
          font-family: 'Karma', sans-serif;
          font-size: 26px;
          font-weight: 500;
          color: #0066D4;
          margin: 0;
      }

  .icon {
      width: 35px;
      height: 40px;
      margin-right: 10px;
  }

  .signup-option {
      font-size: 14px;
      color: #333;
  }

      .signup-option a {
          color: #13bef0;
          text-decoration: none;
      }

  .form-content {
      width: 100%;
      max-width: 600px;
      padding: 40px;
      box-sizing: border-box;
      border: 1px solid var(--Border-Primary-100, #EFEFEF);
      border-radius: 10px;
      background-color: white;
  }

  .form-heading {
      margin-top: 0;
      margin-bottom: 20px;
      font-size: 24px;
      font-weight: 600;
      color: #333;
      text-align: left;
  }

  .input-group {
      margin-bottom: 15px;
  }

      .input-group label {
          display: block;
          margin-bottom: 5px;
          color: #6D6D6D;
      }

      .input-group input {
          width: 100%;
          padding: 10px;
          box-sizing: border-box;
          border-radius: 4px;
          font-family: var(--sans-serif-font);
          color: #474545;
          font-size: var(--delta-font-size);
      }

  .password-wrapper {
      position: relative;
  }

  .pi {
      padding: 4px 10px 0px 10px;
  }

  .password-wrapper input {
      padding-right: 30px;
      border-radius: 4px;
  }

  .password-wrapper button {
      position: absolute;
      right: 0;
      top: 9px;
      background: transparent;
      border: none;
      cursor: pointer;
  }

  .forgot-password {
      color: #0066D4;
  }

  .options {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding-bottom: 20px;
  }

  .login-button {
      font-weight: 600;
      font-size: 16px;
      width: 100%;
      padding: 10px;
      background-color: #13bef0;
      color: white;
      border: none;
      cursor: pointer;
      border-radius: 4px;
  }

  .login-button:disabled {
      background-color: #91d8f7; 
      cursor: not-allowed;
  }

  .google-signup {
      text-align: center;
      font-weight: 400;
      font-size: 14px;
  }

  .google-button {
      font-weight: 500;
      font-size: 14px;
      display: flex;
      align-items: center;
      justify-content: center;
      background: #F9F9F9;
      border: 1px solid #ccc;
      padding: 10px;
      cursor: pointer;
      width: 100%;
      border-radius: 4px;
  }

  .login-button:hover {
      background-color: #005bb5;
  }

  .google-button:hover {
      background-color: #dcdfe1;
  }

  .google-button img {
      width: 20px;
      height: 20px;
      margin-right: 10px;
  }

  .pi-spinner {
      display: inline-block;
      font-size: 16px;
      margin-right: 5px;
  }

  .pi-spin {
      animation: spin 1s infinite linear;
  }

  @keyframes spin {
      from { transform: rotate(0deg); }
      to { transform: rotate(360deg); }
  }

  @media screen and (max-width: 1020px) {
      .login-container {
          display: grid;
          grid-template-columns: auto;
          overflow: auto;
      }

      .header-container {
          flex-direction: column;
          align-items: center;
          padding: 10px;
      }

      .form-content {
          padding: 20px;
          width: 90%;
          margin: 20px auto;
      }

      .options{
          flex-direction: column;
          align-items: start;
      }

      .forgot-password{
          margin-top: 10px;
      }
      .logo{
          margin-right: 65px;
        }
  }
 
</style>