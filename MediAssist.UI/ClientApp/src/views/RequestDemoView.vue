<template>
  <div class="website">
    <HeroBanner
      title="Request your Demo"
      :description="`Discover how ${store.MediAssistConfigManager.DomainName} can enhance your clinical documentation. Our specialists are ready to demonstrate the capabilities of our AI-powered scribe.`"
      v-scroll-reveal
    />

    <div
      class="contact-section"
      id="contactSection"
      ref="contactSection"
      v-scroll-reveal
    >
      <div class="contact-form" v-if="!isSuccess">
        <div class="contact-form-heading-text">
          <div
            v-if="isError"
            ref="errorMessageDiv"
            class="contact-us-error-message"
          >
            {{ errorMessage }}
          </div>
          <h4 class="contact-form-header h4-medium">
            Fill out your details to schedule a personalized demo
          </h4>
        </div>
        <form @submit.prevent="submitForm">
          <div class="form-textboxes">
            <div class="row-fields">
              <div class="form-group">
                <label for="name" class="sm-medium">
                  Name<span class="required">*</span>
                </label>
                <input
                  v-model="name"
                  type="text"
                  id="name"
                  :class="{ error: invalidName }"
                  @blur="validateName"
                  @input="clearErrorMessages('name')"
                  required
                />
                <div class="error-message">
                  {{ invalidName ? nameErrorMessage : "" }}
                </div>
              </div>

              <div class="form-group">
                <label for="email" class="sm-medium"
                  >Email<span class="required">*</span></label
                >
                <input
                  v-model="email"
                  type="text"
                  id="email"
                  :class="{ error: invalidEmail }"
                  @blur="validateEmail"
                  @input="clearErrorMessages('email')"
                  required
                />
                <div class="error-message">
                  {{ invalidEmail ? emailErrorMessage : "" }}
                </div>
              </div>

              <div class="form-group">
                <label for="phone" class="sm-medium">
                  Phone Number<span class="required">*</span>
                </label>
                <div class="phone-container">
                  <select
                    v-model="countryCode"
                    class="input-group input country-code md-regular"
                    :class="{ error: invalidPhone }"
                    @change="checkPhoneNumber"
                  >
                    <option value="+1">+1 (US)</option>
                    <option value="+44">+44 (UK)</option>
                    <option value="+91">+91 (IND)</option>
                    <option value="+971">+971 (UAE)</option>
                  </select>
                  <input
                    v-model="phoneNumber"
                    type="tel"
                    id="phone"
                    :class="{ error: invalidPhone }"
                    @blur="validatePhoneNumber"
                    @input="clearErrorMessages('phone')"
                    required
                  />
                </div>
                <div class="error-message">
                  {{ invalidPhone ? phoneErrorMessage : "" }}
                </div>
              </div>
            </div>

            <div class="form-group">
              <label for="requirements" class="sm-medium">
                Additional Notes / Requirements
              </label>
              <textarea
                class="sm-regular"
                v-model="requirements"
                id="requirements"
                maxlength="1000"
                :class="{ error: invalidRequirements }"
                @blur="validateRequirements"
                @input="clearErrorMessages('requirements')"
                placeholder="Please describe your specific requirements or questions"
              ></textarea>
              <div class="error-message">
                {{ invalidRequirements ? requirementsErrorMessage : "" }}
              </div>
            </div>
          </div>

          <button
            type="submit"
            class="submit-btn"
            :disabled="!isValid || isLoading"
          >
            <span v-if="isLoading">
              <i class="pi pi-spinner pi-spin"></i> Loading...
            </span>
            <span v-else>Schedule Demo</span>
          </button>
        </form>
      </div>

      <div
        class="contact-success"
        ref="successMessageDiv"
        v-if="isSuccess && !isError"
      >
        <div class="contact-form-heading-text">
          <div class="contact-success-image">
            <img
              src="@/assets/contact-success-image.png"
              class="contact-success-icon"
              alt="Contact Us Success"
            />
          </div>
          <div class="contact-success-text">
            <h4 class="contact-us-success-text-title h4-medium">
              Thank you for reaching out to us!
            </h4>
            <div class="contact-us-success-message lg-regular">
              We have received your message and will respond shortly
            </div>
            <button class="contact-us-success-btn" @click="contactSuccess">
              Done
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import axios from "axios";
import { computed, ref, onUnmounted, nextTick } from "vue";
import { PhoneNumberUtil } from "google-libphonenumber";
import { useRouter } from "vue-router";
import { useMyStore } from "@/store/store.ts";
import HeroBanner from "@/components/HeroBanner.vue";

const store = useMyStore();
const name = ref("");
const email = ref("");
const phoneNumber = ref("");
const countryCode = ref("+1");
const requirements = ref("");
const invalidName = ref(false);
const invalidEmail = ref(false);
const invalidPhone = ref(false);
const invalidRequirements = ref(false);
const nameErrorMessage = ref("");
const emailErrorMessage = ref("");
const phoneErrorMessage = ref("");
const requirementsErrorMessage = ref("");
const isLoading = ref(false);
const isSuccess = ref(false);
const isError = ref(false);
const errorMessage = ref("");
const successMessage = ref("");
const errorMessageDiv = ref(null);
const successMessageDiv = ref(null);
const contactSection = ref(null);
const modalType = ref(localStorage.getItem("modalType") || "request-demo");

const router = useRouter();

const showNameField = computed(() => modalType.value === "request-demo");

onUnmounted(() => {
  localStorage.removeItem("modalType");
});

const isValid = computed(
  () =>
    email.value.trim() &&
    (!showNameField.value || name.value.trim()) &&
    (!showNameField.value || phoneNumber.value.trim()) &&
    !invalidEmail.value &&
    (!showNameField.value || !invalidName.value) &&
    (!showNameField.value || !invalidPhone.value) &&
    (!showNameField.value || !invalidRequirements.value)
);

const submitForm = async () => {
  isError.value = false;
  errorMessage.value = "";

  validateFormFields();

  if (isFormValid()) {
    isLoading.value = true;

    try {
      await new Promise((resolve) => setTimeout(resolve, 2000));
      const { apiEndpoint, payload } = getApiEndpointAndPayload();

      const response = await axios.post(apiEndpoint, payload, {
        headers: {
          "Content-Type": "application/json",
        },
      });

      handleResponse(response);
    } catch (error) {
      isError.value = true;
      errorMessage.value =
        "An unexpected error occurred while sending your message. Please try again later.";
      console.error("Error processing request: ", error);

      await nextTick(() => {
        if (errorMessageDiv.value) {
          errorMessageDiv.value.scrollIntoView({
            behavior: "smooth",
            block: "center",
          });
        }
      });
    } finally {
      isLoading.value = false;
    }
  }
};

const validateFormFields = () => {
  if (showNameField.value) {
    validateName();
    validatePhoneNumber();
    validateRequirements();
  }
  validateEmail();
};

const validateName = () => {
  name.value = name.value.trim();

  if (!name.value) {
    nameErrorMessage.value =
      "Name field cannot be left blank. Please enter your name.";
    invalidName.value = true;
  } else if (name.value.length < 3) {
    nameErrorMessage.value = "Name must be at least 3 characters long.";
    invalidName.value = true;
  } else if (!/^[a-zA-Z\s.]+$/.test(name.value)) {
    nameErrorMessage.value = "Name can only contain letters and spaces.";
    invalidName.value = true;
  } else {
    nameErrorMessage.value = "";
    invalidName.value = false;
  }
};

const checkPhoneNumber = () => {
  if (phoneNumber.value) {
    validatePhoneNumber();
  }
};

const validatePhoneNumber = () => {
  phoneNumber.value = phoneNumber.value.trim();

  if (!phoneNumber.value || phoneNumber.value === "") {
    invalidPhone.value = true;
    phoneErrorMessage.value = "Phone Number is required.";
    return;
  }

  const phoneRegex = /^[+\d]+$/;

  if (!phoneRegex.test(phoneNumber.value)) {
    invalidPhone.value = true;
    phoneErrorMessage.value = "Phone Number contains invalid characters.";
    return;
  }

  if (phoneNumber.value.length < 5) {
    invalidPhone.value = true;
    phoneErrorMessage.value = "Phone number is too short.";
    return;
  }

  var phoneNumberWithCountryCode = countryCode.value + phoneNumber.value;
  const phoneUtil = PhoneNumberUtil.getInstance();
  const parsedNumber = phoneUtil.parse(phoneNumberWithCountryCode);

  if (!phoneUtil.isValidNumber(parsedNumber)) {
    invalidPhone.value = true;
    phoneErrorMessage.value =
      "Please enter a valid phone number for the selected country.";
  } else {
    invalidPhone.value = false;
    phoneErrorMessage.value = "";
  }

  return;
};

const validateRequirements = () => {
  requirements.value = requirements.value.trim();

  if (
    requirements.value &&
    (requirements.value.length < 10 || requirements.value.length > 1000)
  ) {
    requirementsErrorMessage.value =
      "Requirements must be between 10 and 1000 characters long.";
    invalidRequirements.value = true;
  } else {
    requirementsErrorMessage.value = "";
    invalidRequirements.value = false;
  }
};

const validateEmail = () => {
  email.value = email.value.trim();
  const emailPattern =
    /^[a-zA-Z0-9][\w.-]*[a-zA-Z0-9]@[a-zA-Z0-9]+[\w.-]*[a-zA-Z0-9]\.[a-zA-Z]{2,}$/;

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
    emailErrorMessage.value =
      "Email field cannot be left blank. Please enter your email.";
    invalidEmail.value = true;
  } else if (localPart.length < 6 || localPart.length > 30) {
    emailErrorMessage.value =
      "The username must be between 6 and 30 characters long.";
    invalidEmail.value = true;
  } else if (!emailPattern.test(email.value)) {
    emailErrorMessage.value = "Please enter a valid email address.";
    invalidEmail.value = true;
  } else if (disposableEmailProviders.includes(domain.toLowerCase())) {
    emailErrorMessage.value =
      "The email domain is not allowed. Please use a valid provider.";
    invalidEmail.value = true;
  } else {
    emailErrorMessage.value = "";
    invalidEmail.value = false;
  }
};

const isFormValid = () => {
  return (
    !invalidEmail.value &&
    (!showNameField.value || !invalidName.value) &&
    (!showNameField.value || !invalidPhone.value) &&
    (!showNameField.value || !invalidRequirements.value)
  );
};

const getApiEndpointAndPayload = () => {
  const apiEndpoints = {
    "request-demo": "requestdemo",
  };

  const payloadMappings = {
    "request-demo": () => ({
      Name: name.value,
      Email: email.value,
      CountryCode: countryCode.value,
      Phone: phoneNumber.value,
      Requirements: requirements.value,
    }),
  };

  return {
    apiEndpoint: `api/manage/${apiEndpoints[modalType.value]}`,
    payload: payloadMappings[modalType.value]?.() || {},
  };
};

const handleResponse = async (response) => {
  if (response.status === 200 && response.data.success) {
    isSuccess.value = true;
    successMessage.value = response.data.message;
    resetFormFields();
    await nextTick(() => {
      if (successMessageDiv.value) {
        successMessageDiv.value.scrollIntoView({
          behavior: "smooth",
          block: "center",
        });
      }
    });
  } else {
    isError.value = true;
    errorMessage.value =
      "An unexpected error occurred while sending your message. Please try again later.";
    console.error("Error from API: ", response.data.message);
  }
};

const resetFormFields = () => {
  email.value = "";
  name.value = "";
  phoneNumber.value = "";
  requirements.value = "";
};

const clearErrorMessages = (field) => {
  switch (field) {
    case "name": {
      if (name.value) {
        nameErrorMessage.value = "";
        invalidName.value = false;
      }
      break;
    }
    case "email": {
      if (email.value) {
        emailErrorMessage.value = "";
        invalidEmail.value = false;
      }
      break;
    }
    case "phone": {
      if (phoneNumber.value) {
        phoneErrorMessage.value = "";
        invalidPhone.value = false;
      }
      break;
    }
    case "requirements": {
      if (requirements.value) {
        requirementsErrorMessage.value = "";
        invalidRequirements.value = false;
      }
      break;
    }
  }
};

const contactSuccess = () => {
  router.push("/");
};
</script>

<style scoped>
.contact-section {
  background: #f7fbff;
  display: flex;
  padding: 60px 120px;
  flex-direction: column;
  align-items: flex-start;
  gap: 10px;
  align-self: stretch;

  @media (min-width: 481px) {
    height: inherit;
    align-items: center;
  }

  @media (min-width: 1440px) {
    margin-inline: auto;
  }
}

.contact-info {
  flex: 1;
  padding: 40px;
  background: #f7fbff;
  border-radius: 8px;
  width: 430px;
}

.contact-info-header {
  font-weight: 500;
  font-size: 32px;
  line-height: 48px;
}

.contact-info-text {
  font-weight: 400;
  font-size: 18px;
  line-height: 31.9px;
}

.support-info-header {
  font-weight: 500;
  font-size: 24px;
  line-height: 31.9px;
}

.support-mail-number {
  gap: 16px;
  display: flex;
  flex-direction: column;
}

.support-email,
.input-group {
  display: flex;
  flex-direction: column;
}

.input-group input {
  height: 20px;
  padding: 16px 12px;
  border-radius: 4px;
  background: #ffffff;
}

.phone-container {
  display: flex;
  width: 115%;
  border-radius: 4px;
  border: 1px solid var(--Border-Primary-100, #efefef);
  overflow: hidden;
}

.phone-container .country-code {
  border: none;
  border-radius: 4px;
  padding: 11px 12px;
  width: fit-content;
  appearance: none;
  -webkit-appearance: none;
  -moz-appearance: none;
  background-image: url("@/assets/Alt-Arrow-Down.svg");
  background-repeat: no-repeat;
  background-position: right 8px center;
  background-size: 19px;
  padding-right: 30px;
}

.phone-container input {
  flex: 1;
  border: none;
  border-radius: 4px;
}

.support-email-text-icon {
  gap: 8px;
  display: flex;
  font-weight: 400;
  font-size: 18px;
  line-height: 31.9px;
}

.support-email-number-icon {
  width: 17px;
  height: 17px;
  top: 6.25px;
  left: 1.25px;
  position: relative;
}

.support-email-number-text {
  font-weight: 400;
  font-size: 18px;
  line-height: 31.9px;
  margin: 0px;
}

.contact-mail {
  padding-left: 24px;
  text-decoration: none;
  color: #0066d4;
}

.contact-number {
  padding-left: 24px;
  margin: 0px;
}

.contact-form {
  flex: 1;
  background: #ffffff;
  position: relative;
  border-radius: 16px;
  display: flex;
  flex-direction: column;
  width: 1186px;
  padding: 48px;
  align-items: flex-start;
  gap: 40px;
}

.contact-form-heading-text {
  gap: 16px;
  display: flex;
  flex-direction: column;
}

.contact-us-error-message {
  color: red;
}

.contact-form-header {
  margin: 0px;
  color: #000;
}

.contact-form-text {
  font-weight: 400;
  font-size: 18px;
  line-height: 31.9px;
  margin: 0px;
  color: #606060;
}

.form-textboxes {
  gap: 40px;
  display: flex;
  flex-direction: column;
}

.row-fields {
  display: flex;
  flex-direction: row;
  align-items: flex-start;
  gap: 40px;
  align-self: stretch;
  width: 126%;
}

.form-group {
  /* 
    gap: 8px;
    display: flex;
    flex-direction: column;
     */
  position: relative;
  width: 100%;
  color: #7e7e7e;
}

.required {
  color: #fd1414;
  font-size: 14px;
  font-style: normal;
  font-weight: 700;
  line-height: 150%;
}

.error-message {
  color: red;
  font-size: 12px;
  position: absolute;
  left: 0;
}

input,
textarea {
  width: 100%;
  padding: 14px;
  border-radius: 4px;

  box-sizing: border-box;
}

textarea {
  resize: vertical;
  max-height: 200px;
  min-height: 100px;
  overflow-y: auto;
  width: 132% !important;
  margin-top: 5px;
}

.submit-btn {
  background: linear-gradient(274.4deg, #0066d4 2.36%, #00356e 97.81%);
  color: white;
  border: none;
  cursor: pointer;
  display: flex;
  height: 40px;
  padding: 16px;
  flex-direction: column;
  justify-content: center;
  width: 153px;
  border-radius: 7.82px;
  border-width: 0.98px;
  align-items: center;
  margin-top: 15px;
}

.submit-btn:hover {
  background: linear-gradient(0deg, #2d4f9f, #2d4f9f),
    linear-gradient(274.4deg, #0057b4 2.36%, #00254d 97.81%);
  cursor: pointer;
  font-weight: bolder;
}

button:disabled {
  background: #f6f6f6;
  color: #7e7e7e;
  cursor: not-allowed;
  opacity: 0.7;

  width: 153px;
  height: 40px;
  border-radius: 7.82px;
  border-width: 0.98px;
  padding: 16px;
  align-items: center;
}

button:disabled:hover {
  background: #f6f6f6;
  color: #7e7e7e;
  cursor: not-allowed;
  opacity: 0.7;
}

.contact-success {
  background: #ffffff;
  flex: 1;
  position: relative;
  width: 1241px;
  gap: 24px;
  border-radius: 16px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  height: 430px;
  padding: 48px;
}

.contact-success-image {
  justify-content: center;
  display: flex;
  margin: 0px;
}

.contact-success-icon {
  width: 120px;
  height: 120px;
}

.contact-us-success-text-title {
  text-align: center;
  margin: 0px;
}

.contact-success-text {
  text-align: -webkit-center;
}

.contact-us-success-message {
  text-align: center;
  margin: 0px;
  width: 335px;
  position: relative;
  color: #606060;
}

.contact-us-success-btn {
  margin-top: 20px;
  width: 81px;
  height: 40px;
  padding-top: 5px;
  padding-right: 10px;
  padding-bottom: 5px;
  padding-left: 10px;
  border-radius: 8px;
  border-width: 1px;
  background: #ffffff;
  border: 1px solid #efefef;
  cursor: pointer;
}

textarea::placeholder {
  font-family: "Inter";
}
</style>
