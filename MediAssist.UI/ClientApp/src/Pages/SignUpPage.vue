<template>
  <div class="website signup-container">
    <div class="image-side">
      <img src="@/assets/loginPageImage.jpg" alt="SignUp" />
    </div>

    <div class="form-side">
      <header class="header-container">
        <div class="logo">
          <AppLogo />
        </div>

        <p class="signup-option">
          Already have an account?
          <a href="/login" @click="redirectToLogin">SignIn</a>
        </p>
      </header>

      <div class="form-content">
        <div v-if="successMessage" class="success-message">
          {{ successMessage }}
        </div>
        <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
        <h2 class="form-heading">Sign Up</h2>

        <form @submit.prevent="handleSignUp">
          <div class="input-group double-row">
            <div class="input-field">
              <label for="title">Title</label>
              <select
                class="dropdown-field"
                name="title"
                id="title"
                v-model.number="title"
                @focus="interactWithField('title')"
                @blur="validateField('title')"
                required
              >
                <option :value="1">Dr. (Doctor)</option>
                <option :value="2">Consultant</option>
                <option :value="3">Resident</option>
                <option :value="4">Attending Physician</option>
                <option :value="5">Senior Consultant</option>
                <option :value="6">Chief Surgeon</option>
                <option :value="7">Clinical Lead</option>
              </select>
              <span class="error">{{ errors.title }}</span>
            </div>
            <div class="input-field name-dob-field">
              <label for="name">Full Name</label>
              <input
                v-model="name"
                type="text"
                id="name"
                @focus="interactWithField('name')"
                @blur="validateField('name')"
                required
              />
              <span class="error">{{ errors.name }}</span>
            </div>
          </div>

          <div class="input-group double-row">
            <div class="input-field">
              <label for="gender">Gender</label>
              <select
                class="dropdown-field"
                name="gender"
                id="gender"
                v-model.number="gender"
                @focus="interactWithField('gender')"
                @blur="validateField('gender')"
              >
                <option :value="1">Male</option>
                <option :value="2">Female</option>
                <option :value="3">Transgender</option>
                <option :value="4">Non-binary</option>
                <option :value="5">Prefer Not to Say</option>
              </select>
              <span class="error">{{ errors.gender }}</span>
            </div>
            <div class="input-field name-dob-field">
              <label for="dob">Date of Birth</label>
              <input
                type="date"
                name="dob"
                id="dob"
                v-model="dob"
                class="dob-picker"
                @focus="interactWithField(dob)"
                @blur="validateDob(dob)"
                @input="validateDob(dob)"
                :max="getCurrentDate()"
              />
              <span v-if="dobErrors.blankDob" class="error">
                Date of Birth cannot be left blank.
              </span>
              <span v-if="dobErrors.invalidDateFormat" class="error">
                Please enter a valid date.
              </span>
              <span v-if="dobErrors.invalidAge" class="error">
                The age must be above 18 years.
              </span>
              <span v-if="dobErrors.futureDate" class="error">
                Date of birth cannot be a future date.
              </span>
            </div>
          </div>

          <div class="input-group">
            <label for="email">Email</label>
            <input
              v-model="email"
              type="text"
              id="email"
              @paste="handlePaste"
              @input="trimEmail"
              @focus="interactWithField('email')"
              @blur="emailFieldCheck"
              required
            />
            <span v-if="errors.email" class="error">{{ errors.email }}</span>
          </div>

          <div class="input-group password-group">
            <label for="password">Password</label>
            <div class="password-wrapper">
              <input
                v-model="password"
                :type="passwordVisible ? 'text' : 'password'"
                id="password"
                @focus="handleFocus()"
                @blur="handleBlur()"
                @input="validatePassword"
                autocomplete="new-password"
                required
              />
              <button
                v-if="isPasswordInputActive"
                type="button"
                @click="togglePasswordVisibility"
              >
                <span v-if="passwordVisible"
                  ><i class="pi pi-eye-slash"></i
                ></span>
                <span v-else><i class="pi pi-eye"></i></span>
              </button>
              <div class="message-tooltip" v-if="tooltipVisible">
                <div class="message-content">
                  <p>Password must include:</p>
                  <ul>
                    <li :class="{ valid: isLengthValid }">
                      <i
                        :class="{
                          'pi pi-check': isLengthValid,
                          'pi pi-times': !isLengthValid,
                        }"
                      ></i>
                      At least 8 characters
                    </li>
                    <li :class="{ valid: hasUpperCase }">
                      <i
                        :class="{
                          'pi pi-check': hasUpperCase,
                          'pi pi-times': !hasUpperCase,
                        }"
                      ></i>
                      One uppercase letter
                    </li>
                    <li :class="{ valid: hasLowerCase }">
                      <i
                        :class="{
                          'pi pi-check': hasLowerCase,
                          'pi pi-times': !hasLowerCase,
                        }"
                      ></i>
                      One lowercase letter
                    </li>
                    <li :class="{ valid: hasDigit }">
                      <i
                        :class="{
                          'pi pi-check': hasDigit,
                          'pi pi-times': !hasDigit,
                        }"
                      ></i>
                      One digit
                    </li>
                    <li :class="{ valid: hasSpecialChar }">
                      <i
                        :class="{
                          'pi pi-check': hasSpecialChar,
                          'pi pi-times': !hasSpecialChar,
                        }"
                      ></i>
                      One special character
                    </li>
                  </ul>
                </div>
                <div class="message-arrow"></div>
              </div>
            </div>
            <span v-if="errors.password" class="error">{{
              errors.password
            }}</span>
          </div>

          <div class="input-group">
            <label for="confirmPassword">Confirm Password</label>
            <div class="password-wrapper">
              <input
                v-model="confirmPassword"
                :type="ConfirmpasswordVisible ? 'text' : 'password'"
                id="confirmPassword"
                @focus="interactWithField('confirmPassword')"
                @blur="validateField('confirmPassword')"
                @input="handleConfirmPasswordInput()"
                autocomplete="off"
                required
              />

              <button
                v-if="isConfirmPasswordInputActive"
                type="button"
                @click="toggleConfirmpasswordVisibleVisibility"
              >
                <span v-if="ConfirmpasswordVisible"
                  ><i class="pi pi-eye-slash"></i
                ></span>
                <span v-else><i class="pi pi-eye"></i></span>
              </button>
            </div>
            <span v-if="errors.confirmPassword" class="error">{{
              errors.confirmPassword
            }}</span>
          </div>

          <div class="input-group">
            <label for="medicalCredentials">Medical Credentials</label>
            <select
              class="medCred-field"
              name="medicalCredentials"
              id="medicalCredentials"
              v-model.number="medicalCredentials"
              @focus="interactWithField('medicalCredentials')"
              @blur="validateField('medicalCredentials')"
              required
            >
              <option :value="1">MD (Doctor of Medicine)</option>
              <option :value="2">
                MBBS (Bachelor of Medicine, Bachelor of Surgery)
              </option>
              <option :value="3">DO (Doctor of Osteopathic Medicine)</option>
              <option :value="4">BDS (Bachelor of Dental Surgery)</option>
              <option :value="5">MCh (Master of Surgery)</option>
              <option :value="6">DM (Doctorate of Medicine)</option>
              <option :value="7">
                FRCS (Fellowship of the Royal College of Surgeons)
              </option>
              <option :value="8">
                FACP (Fellow of the American College of Physicians)
              </option>
              <option :value="9">MS (Master of Surgery)</option>
              <option :value="10">DNB (Diplomate of National Board)</option>
            </select>
            <span v-if="errors.medicalCredentials" class="error">{{
              errors.medicalCredentials
            }}</span>
          </div>

          <div class="input-group">
            <label for="specialization">Specialization</label>
            <input
              v-model="specialization"
              type="text"
              id="specialization"
              @focus="interactWithField('specialization')"
              @blur="validateField('specialization')"
              required
            />
            <span v-if="errors.specialization" class="error">{{
              errors.specialization
            }}</span>
          </div>

          <div class="terms-privacy">
            <input
              type="checkbox"
              id="agreeTerms"
              v-model="agreeTerms"
              @focus="interactWithField('agreeTerms')"
              @change="validateField('agreeTerms')"
              required
            />
            <label for="agreeTerms" class="terms-privacy-license">
              I agree to the
              <a href="/terms-and-conditions" target="_blank"
                >Terms & Conditions</a
              >
              and <a href="/privacy-policy" target="_blank">Privacy Policy</a>.
            </label>
          </div>
          <div class="terms-privacy">
            <input
              type="checkbox"
              id="agreeLicense"
              v-model="agreeLicense"
              @focus="interactWithField('agreeLicense')"
              @change="validateField('agreeLicense')"
              required
            />
            <label for="agreeLicense" class="terms-privacy-license">
              I agree to the
              <a href="/license-agreement" target="_blank">License Agreement</a
              >.
            </label>
          </div>
          <span v-if="errors.agreeTerms && errors.agreeLicense" class="error">
            {{ errors.agreeTerms }}
          </span>
          <span v-else-if="errors.agreeTerms" class="error">
            {{ errors.agreeTerms }}
          </span>
          <span v-else-if="errors.agreeLicense" class="error">
            {{ errors.agreeLicense }}
          </span>

          <button
            type="submit"
            class="signup-button"
            :disabled="!isFormValid || isLoading"
          >
            <span v-if="isLoading">
              <i class="pi pi-spinner pi-spin"></i> Creating...
            </span>
            <span v-else> Create Account </span>
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted, onUnmounted } from "vue";
import { useRouter } from "vue-router";
import AppLogo from "@/components/AppLogo.vue";
import axiosInstance from "@/Services/Interceptors/axios";

const router = useRouter();
const title = ref("");
const name = ref("");
const errorMessage = ref("");
const gender = ref("");
const dob = ref("");
const email = ref("");
const password = ref("");
const confirmPassword = ref("");
const medicalCredentials = ref("");
const specialization = ref("");
const passwordVisible = ref(false);
const ConfirmpasswordVisible = ref(false);
const tooltipVisible = ref(false);
const agreeTerms = ref(false);
const agreeLicense = ref("");
const isPasswordInputActive = ref(false);
const isConfirmPasswordInputActive = ref(false);
const isLengthValid = ref(false);
const hasUpperCase = ref(false);
const hasLowerCase = ref(false);
const hasDigit = ref(false);
const hasSpecialChar = ref(false);
const successMessage = ref("");

const errors = ref({
  title: "",
  name: "",
  gender: "",
  dob: "",
  email: "",
  password: "",
  confirmPassword: "",
  medicalCredentials: "",
  specialization: "",
  agreeTerms: "",
  agreeLicense: "",
});
const interacted = ref({
  title: false,
  name: false,
  gender: false,
  dob: false,
  email: false,
  password: false,
  confirmPassword: false,
  medicalCredentials: false,
  specialization: false,
  agreeTerms: false,
  agreeLicense: false,
});

const dobErrors = ref({
  invalidDateFormat: false,
  invalidAge: false,
  futureDate: false,
  blankDob: false,
});

const getCurrentDate = () => {
  const today = new Date();
  const year = today.getFullYear();
  const month = String(today.getMonth() + 1).padStart(2, "0");
  const day = String(today.getDate()).padStart(2, "0");
  return `${year}-${month}-${day}`;
};
const validateDob = (dob) => {
  const today = new Date();
  const selectedDate = new Date(dob);
  dobErrors.value = {
    invalidDateFormat: false,
    invalidAge: false,
    futureDate: false,
    blankDob: false,
  };
  if (dob.value == "") {
    dobErrors.value.blankDob = true;
    return;
  }

  if (isNaN(selectedDate.getTime())) {
    dobErrors.value.invalidDateFormat = true;
    return;
  }
  if (selectedDate > today) {
    dobErrors.value.futureDate = true;
    return;
  }
  const age = calculateAge(dob);
  if (age < 18) {
    dobErrors.value.invalidAge = true;
  }
};
const calculateAge = (dob) => {
  const birthDate = new Date(dob);
  const today = new Date();
  let age = today.getFullYear() - birthDate.getFullYear();
  const monthDifference = today.getMonth() - birthDate.getMonth();
  if (
    monthDifference < 0 ||
    (monthDifference === 0 && today.getDate() < birthDate.getDate())
  ) {
    age--;
  }
  return age;
};

onMounted(() => {
  successMessage.value = sessionStorage.getItem("successMessage") || "";
  sessionStorage.removeItem("successMessage");
  document.addEventListener("click", clearMessage);
});

onUnmounted(() => {
  document.removeEventListener("click", clearMessage);
});

const togglePasswordVisibility = () => {
  passwordVisible.value = !passwordVisible.value;
};

const toggleConfirmpasswordVisibleVisibility = () => {
  ConfirmpasswordVisible.value = !ConfirmpasswordVisible.value;
};

const checkConfirmPasswordInput = () => {
  isConfirmPasswordInputActive.value = confirmPassword.value.trim().length > 0;
};

const handleConfirmPasswordInput = () => {
  interactWithField("confirmPassword");
  checkConfirmPasswordInput();
  if (errors.value.confirmPassword) {
    errors.value.confirmPassword = "";
  }
};

const showTooltip = () => {
  tooltipVisible.value = true;
};

const hideTooltip = () => {
  tooltipVisible.value = false;
};

const handleFocus = () => {
  interactWithField("password");
  showTooltip();
};

const handleBlur = () => {
  validateField("password");
  hideTooltip();
};

const validatePassword = () => {
  const pwd = password.value;
  isPasswordInputActive.value = password.value.trim().length > 0;
  isLengthValid.value = pwd.length >= 8;
  hasUpperCase.value = /[A-Z]/.test(pwd);
  hasLowerCase.value = /[a-z]/.test(pwd);
  hasDigit.value = /\d/.test(pwd);
  hasSpecialChar.value = /[!@#$%^&*(),.?":{}|<>_+\-=\\`~[\]'/;]/.test(pwd);

  if (interacted.value.password) {
    showTooltip();
  } else {
    hideTooltip();
  }
};
const clearMessage = () => {
  successMessage.value = "";
  sessionStorage.removeItem("successMessage");
  errorMessage.value = "";
};
const trimEmail = () => {
  email.value = email.value.trim();
};

const handlePaste = () => {
  setTimeout(() => {
    email.value = email.value.trim();
  }, 0);
};

const interactWithField = (field) => {
  interacted.value[field] = true;
};

const validateField = (field) => {
  if (!interacted.value[field]) {
    return;
  }

  const validators = {
    title: validateTitle,
    name: validateName,
    gender: validateGender,
    email: validateEmail,
    password: validatesPassword,
    confirmPassword: validateConfirmPassword,
    medicalCredentials: validateMedicalCredentials,
    specialization: validateSpecialization,
    agreeTerms: validateAgreements,
    agreeLicense: validateAgreements,
  };

  const validator = validators[field];
  if (validator) {
    validator();
  }
};

const validateTitle = () => {
  errors.value.title = !title.value ? "Title is required" : "";
};

const validateName = () => {
  name.value = name.value.trim();
  const namePattern = /^[A-Za-z\s.]+$/;
  if (!name.value) {
    errors.value.name = "Name is required.";
  } else if (!namePattern.test(name.value)) {
    errors.value.name = "Name must contain only alphabetic characters.";
  } else if (name.value.length < 3) {
    errors.value.name = "Name must be at least 3 characters long.";
  } else {
    errors.value.name = "";
  }
};

const validateGender = () => {
  errors.value.gender = !gender.value ? "Gender is a required field." : "";
};

const validateEmail = () => {
  const emailPattern =
    /^[a-zA-Z0-9][\w.-]*[a-zA-Z0-9]@[a-zA-Z0-9]+[\w.-]*[a-zA-Z0-9]\.[a-zA-Z]{2,}$/;
  const spamPattern = /^\d+$/;
  const disposableEmailProviders = [
    "yopmail.com",
    "mailinator.com",
    "guerrillamail.com",
    "10minutemail.com",
    "aol.com",
    "example.com",
    "a.com",
    "test.com",
  ];
  const [localPart, domain] = email.value.split("@");

  if (!email.value) {
    errors.value.email =
      "Email field cannot be left blank. Please enter your email.";
  } else if (localPart.length < 6 || localPart.length > 30) {
    errors.value.email =
      "The username must be between 6 and 30 characters long.";
  } else if (!emailPattern.test(email.value)) {
    errors.value.email = "Please enter a valid email address.";
  } else if (disposableEmailProviders.includes(domain.toLowerCase())) {
    errors.value.email =
      "The email domain you have entered is not allowed. Please use a valid email provider.";
  } else if (spamPattern.test(localPart)) {
    errors.value.email =
      "Sorry, usernames of 8 or more characters must include at least one alphabetical character";
  } else if (domain.length === 5 || localPart.toLowerCase() === "user") {
    errors.value.email = "Suspicious email addresses are not allowed.";
  } else {
    errors.value.email = "";
  }
};

const validatesPassword = () => {
  if (!password.value) {
    errors.value.password = "Password is required.";
  } else if (password.value.length < 8) {
    errors.value.password = "Password must be at least 8 characters long.";
  } else if (!isLengthValid.value) {
    errors.value.password = "Password must include at least 8 characters.";
  } else if (!hasUpperCase.value) {
    errors.value.password =
      "Password must include at least one uppercase letter.";
  } else if (!hasLowerCase.value) {
    errors.value.password =
      "Password must include at least one lowercase letter.";
  } else if (!hasDigit.value) {
    errors.value.password = "Password must include at least one digit.";
  } else if (!hasSpecialChar.value) {
    errors.value.password =
      "Password must include at least one special character.";
  } else {
    errors.value.password = "";
  }
};

const validateConfirmPassword = () => {
  if (!confirmPassword.value) {
    errors.value.confirmPassword = "Please confirm your password.";
  } else if (confirmPassword.value !== password.value) {
    errors.value.confirmPassword = "Passwords do not match.";
    confirmPassword.value = "";
  } else {
    errors.value.confirmPassword = "";
  }
};

const validateMedicalCredentials = () => {
  if (!medicalCredentials.value) {
    errors.value.medicalCredentials = "This field is required.";
  } else if (medicalCredentials.value.length > 100) {
    errors.value.medicalCredentials =
      "This field cannot exceed 100 characters.";
  } else {
    errors.value.medicalCredentials = "";
  }
};

const validateSpecialization = () => {
  specialization.value = specialization.value.trim();
  if (!specialization.value) {
    errors.value.specialization = "This field is required.";
  } else if (specialization.value.length > 100) {
    errors.value.specialization = "This field cannot exceed 100 characters.";
  } else {
    errors.value.specialization = "";
  }
};

const validateAgreements = () => {
  const bothChecked = agreeTerms.value && agreeLicense.value;

  if (!interacted.value.agreeTerms || !interacted.value.agreeLicense) {
    errors.value.agreeTerms = "";
    errors.value.agreeLicense = "";
    return;
  }

  if (!bothChecked) {
    if (!agreeTerms.value && !agreeLicense.value) {
      errors.value.agreeTerms =
        "Please agree to both the checkboxes to complete the registration.";
      errors.value.agreeLicense =
        "Please agree to both the checkboxes to complete the registration.";
    } else if (agreeTerms.value && !agreeLicense.value) {
      errors.value.agreeLicense =
        "Please agree to the License Agreement to continue.";
      errors.value.agreeTerms = "";
    } else if (!agreeTerms.value && agreeLicense.value) {
      errors.value.agreeTerms =
        "Please agree to the Terms & Conditions and Privacy Policy to continue.";
      errors.value.agreeLicense = "";
    } else {
      errors.value.agreeTerms = "";
      errors.value.agreeLicense = "";
    }
  } else {
    errors.value.agreeTerms = "";
    errors.value.agreeLicense = "";
  }
};

const isLoading = ref(false);

const handleSignUp = async () => {
  validateField("title");
  validateField("name");
  validateField("gender");
  validateDob(dob.value);
  validateField("email");
  validateField("password");
  validateField("confirmPassword");
  validateField("medicalCredentials");
  validateField("specialization");
  validateField("agreeTerms");
  validateField("agreeLicense");
  const hasErrors = Object.values(dobErrors.value).some(
    (error) => error === true
  );

  if (
    !errors.value.title &&
    !errors.value.name &&
    !errors.value.gender &&
    !hasErrors &&
    !errors.value.email &&
    !errors.value.password &&
    !errors.value.confirmPassword &&
    !errors.value.medicalCredentials &&
    !errors.value.specialization &&
    !errors.value.agreeTerms &&
    !errors.value.agreeLicense
  ) {
    isLoading.value = true;
    try {
      const payload = {
        Title: title.value,
        Name: name.value,
        Gender: gender.value,
        DateOfBirth: dob.value,
        Email: email.value,
        Password: password.value,
        ConfirmPassword: confirmPassword.value,
        MedicalCredentials: medicalCredentials.value,
        Specialization: specialization.value,
        TermsAndPrivacy: agreeTerms.value,
        LicenseAgreement: agreeLicense.value,
      };

      const response = await axiosInstance.post("/api/manage/signup", payload);

      if (response.data.success) {
        sessionStorage.setItem(
          "successMessage",
          "Your account has been successfully created. Please sign in to continue."
        );

        router.push("/login");
      } else {
        errorMessage.value = response.data.message;
      }
    } catch (error) {
      errorMessage.value =
        "There was an error submitting the form. Please try again later.";
    } finally {
      isLoading.value = false;
    }
  }
};

const redirectToLogin = () => {
  router.push("/login");
};

const isFormValid = computed(() => {
  return (
    title.value &&
    name.value &&
    gender.value &&
    dob.value &&
    email.value &&
    password.value &&
    confirmPassword.value &&
    medicalCredentials.value &&
    specialization.value &&
    agreeTerms.value &&
    agreeLicense.value &&
    !errors.value.title &&
    !errors.value.name &&
    !errors.value.gender &&
    !errors.value.dob &&
    !errors.value.email &&
    !errors.value.password &&
    !errors.value.confirmPassword &&
    !errors.value.medicalCredentials &&
    !errors.value.specialization &&
    !errors.value.agreeTerms &&
    !errors.value.agreeLicense
  );
});

const emailFieldCheck = () => {
  validateField("email");
  if (errors.value.email === "") {
    handleEmailCheck();
  }
};

const handleEmailCheck = async () => {
  if (errors.value.email == "") {
    try {
      const response = await axiosInstance.get(
        `/api/manage/emailcheck?emailId=${email.value}`
      );

      if (response.data.ifEmailExist) {
        errors.value.email =
          "This email is already existing. Please try with another.";
      } else {
        errors.value.email = "";
      }
    } catch (error) {
      console.error("Error during checking email:");
    } finally {
      isLoading.value = false;
    }
  }
};

watch(password, validatePassword);
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Karma:wght@400;500&display=swap");

.input-group.double-row {
  display: flex;
  justify-content: space-between;
}

.input-field {
  flex: 0 0 48%;
  box-sizing: border-box;
}

.name-dob-field {
  margin-right: 15px;
  flex-basis: 52% !important;
}

.dropdown-field {
  width: 85%;
  height: 64%;
  box-sizing: border-box;
  border-radius: 4px;
}

.medCred-field {
  width: 100%;
  height: 42px;
  box-sizing: border-box;
  border-radius: 4px;
}

.error {
  color: red;
  font-size: 12px;
  position: relative;
  display: flex;
}

.error-message {
  color: red;
  font-size: 14px;
  margin-bottom: 10px;
}

.success-message {
  font-size: 14px;
  margin-bottom: 10px;
  color: green;
}

.signup-container {
  display: flex;
  flex-direction: row;
  height: 100vh;
  overflow: hidden;
}

.image-side {
  flex: 1;
  height: 100vh;
  position: sticky;
  top: 0;
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
  align-items: center;
  overflow-y: auto;
  height: 100vh;
  padding: 20px;
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
  text-decoration: none;
}

.logo h3 {
  font-size: 26px;
  font-weight: 500;
  color: #0066d4;
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
  border: 1px solid var(--Border-Primary-100, #efefef);
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
  margin-bottom: 25px;
}

.input-group label {
  display: flex;
  align-items: center;
  margin-bottom: 5px;
  color: #6d6d6d;
}

.input-group input {
  width: 100%;
  padding: 10px;
  box-sizing: border-box;
  border-radius: 4px;
  color: #474545;
  font-size: 14px;
}

.password-group {
  position: relative;
}

.password-wrapper {
  position: relative;
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

.pi {
  padding: 4px 10px 0px 0px;
}

.info-icon {
  margin-left: 10px;
  font-size: 14px;
  color: #6d6d6d;
  cursor: pointer;
}

.message-tooltip {
  position: absolute;
  top: -184px;
  left: 85px;
  background: #f7f8fa;
  color: #333;
  border-radius: 10px;
  max-width: 250px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
  font-size: 14px;
  display: block;
}

.message-content {
  padding: 10px;
}

.message-content ul {
  list-style-type: none;
  padding-left: 10px;
  margin: 0;
}

.message-content li {
  margin-bottom: 5px;
  display: flex;
  align-items: center;
}

.message-content li.valid i.pi-check {
  color: green;
}

.message-content i.pi-times {
  color: red;
}

.message-arrow {
  position: absolute;
  bottom: -10px;
  left: 20px;
  width: 0;
  height: 0;
  border-left: 10px solid transparent;
  border-right: 10px solid transparent;
  border-top: 10px solid #f7f8fa;
}

.message-arrow::after {
  content: "";
  position: absolute;
  top: -8px;
  left: 0;
  width: 8px;
  height: 8px;
  background: #f7f8fa;
  transform: rotate(45deg);
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
  from {
    transform: rotate(0deg);
  }

  to {
    transform: rotate(360deg);
  }
}

.signup-button {
  font-weight: 600;
  font-size: 16px;
  width: 100%;
  padding: 10px;
  background-color: #13bef0;
  color: white;
  border: none;
  cursor: pointer;
  border-radius: 4px;
  margin-top: 25px;
}

.signup-button:disabled {
  background-color: #91d8f7;
  cursor: not-allowed;
}

.terms-privacy-license {
  font-size: 13px;
}

.terms-privacy {
  display: flex;
  align-items: center;
  padding-bottom: 5px;
}

.terms-privacy input {
  margin-right: 10px;
}

.terms-privacy a {
  color: #13bef0;
  cursor: pointer;
  text-decoration: none;
}

.terms-privacy a:hover {
  text-decoration: underline;
}

@media (max-width: 1024px) {
  .signup-container {
    display: flex;
    flex-direction: column;
    height: auto;
    overflow: auto;
  }

  .image-side {
    flex: none;
    height: 300px;
    position: relative;
  }

  .form-side {
    flex: none;
    height: auto;
    overflow-y: visible;
  }

  .header-container {
    flex-direction: column;
    align-items: center;
  }

  .form-content {
    padding: 20px;
    border-radius: 8px;
  }

  .form-heading {
    font-size: 20px;
    text-align: center;
  }

  .logo {
    align-items: center;
    margin-bottom: 10px;
    margin-right: 65px;
  }

  .icon {
    width: 30px;
    height: 35px;
    margin-right: 0;
    margin-bottom: 5px;
  }

  .input-group input {
    font-size: 13px;
  }

  .signup-button {
    font-size: 14px;
  }

  .message-tooltip {
    max-width: 200px;
    top: -160px;
  }

  .terms-privacy {
    align-items: flex-start;
  }

  .terms-privacy label {
    margin-left: 0;
    font-size: 12px;
  }
}

@media (max-width: 480px) {
  .image-side img {
    object-fit: cover;
  }

  .form-content {
    padding: 15px;
  }

  .form-heading {
    font-size: 18px;
  }

  .input-group input {
    font-size: 12px;
  }

  .signup-button {
    font-size: 13px;
    padding: 8px;
  }

  .message-tooltip {
    max-width: 180px;
  }

  .terms-privacy {
    font-size: 11px;
  }
}
</style>
