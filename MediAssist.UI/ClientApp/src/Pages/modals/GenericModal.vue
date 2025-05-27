<template>
  <div  class="modal" v-if="isVisible">
    <div :class="showNameField ? 'modal-content-schedule-demo' : 'modal-content'" :style="isSuccess || isError ? { height: '280px', top: '175px' } : {}">
      <span 
        class="close" 
        @click="handleClose" 
        :class="{ disabled: isLoading }"
        :style="{ pointerEvents: isLoading ? 'none' : 'auto' }"
      >&times;</span>
      
      <div v-if="!isSuccess && !isError">
        <h2 class="heading">{{ title }}</h2>
        <form @submit.prevent="submitForm">
          <p class="content" :style="showNameField ? { margin: '0px' } : {}">
            {{ description }}
          </p>
          
          <!-- Name Input (for Schedule a Demo) -->
          <div v-if="showNameField || props.modalType === 'report-send'" class="input-group mb-8">
            <label for="name" class="input-group label">Name<span class="required">*</span></label>
            <input 
              v-model="name"
              type="text"
              id="name"
              class="input-group input"
              :class="{ error: invalidName }"
              @blur="validateName"
              required 
            />
            <div class="error-message">
              {{ invalidName ? nameErrorMessage : '' }}
            </div>
          </div>
          
          <!-- Email Input -->
          <div class="input-group mb-8">
           <div><label for="email" class="input-group label">Email<span class="required">*</span></label></div> 
            <input 
              v-model="email"
              type="text"
              id="email"
              class="input-group input"
              :class="{ error: invalidEmail }"
              @blur="validateEmail"
              required 
            />
            <div class="error-message">
              {{ invalidEmail ? emailErrorMessage : '' }}
            </div>
          </div>
          
          <!-- Phone Number Input (for Schedule a Demo) -->
          <div v-if="showNameField  && props.modalType != 'report-send'" class="input-group phone-input mb-8">
            <label for="phone" class="input-group label">Phone Number<span class="required">*</span></label>
            <div class="phone-container">
              <select 
                v-model="countryCode" 
                class="input-group input country-code" 
                :class="{ error: invalidPhone }"
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
                class="input-group input phone-number input"
                :class="{ error: invalidPhone }"
                @blur="validatePhoneNumber"
                required 
              />
            </div>
            <div class="error-message">
              {{ invalidPhone ? phoneErrorMessage : '' }}
            </div>
          </div>

          <!-- Requirements Input (for Schedule a Demo) -->
          <div v-if="showNameField  && props.modalType != 'report-send'" class="input-group mb-8" style="margin: 0px;">
            <label for="requirements" class="input-group label">Additional Notes / Requirements</label>
            <textarea 
              v-model="requirements"
              id="requirements"
              maxlength="500"
              class="requirements-input"
              :class="{ error: invalidRequirements }"
              @blur="validateRequirements"
              placeholder="Please describe your specific requirements or questions"
            ></textarea>
            <div class="error-message">
              {{ invalidRequirements ? requirementsErrorMessage : '' }}
            </div>
          </div>

          <button 
            type="submit"
            class="submit-button"
            :disabled="!isValid || isLoading"
          >
            <span v-if="isLoading">
              <i class="pi pi-spinner pi-spin"></i> Loading...
            </span>
            <span v-else>{{ submitButtonText }}</span>
          </button>
        </form>
      </div>
      
      <div v-if="isSuccess">
        <div class="mail-sussess">
          <img class="mail-sent" src="@/assets/mail-sent.gif" :alt="successAlt" />
          <p class="success-message">
            {{ successMessage }}
          </p>
        </div>
      </div>

      <div v-if="isError">
        <div class="mail-sussess">
          <img class="mail-sent" src="@/assets/error-image.gif" :alt="errorAlt" />
          <p class="success-message">
            {{ errorMessage }}
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import axios from 'axios';
import { computed, defineEmits, defineProps, onMounted, ref, watch } from 'vue';
import { PhoneNumberUtil } from 'google-libphonenumber';

const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false,
  },
  modalType: {
    type: String,
    default: 'forgot-password',
    validator: (value) => ['forgot-password', 'schedule-demo','report-send'].includes(value)
  },
  title: {
    type: String,
    default: '',
  },
  description: {
    type: String,
    default: '',
  },
  submitButtonText: {
    type: String,
    default: 'Submit Now',
  },
  selectedPlan: {
    type: String,
    default: '',
  },
})

const emit = defineEmits(['close'])

const name = ref('')
const email = ref('')
const phoneNumber = ref('')
const countryCode = ref('+1')
const requirements = ref('')
const invalidName = ref(false)
const invalidEmail = ref(false)
const invalidPhone = ref(false)
const invalidRequirements = ref(false)
const nameErrorMessage = ref('')
const emailErrorMessage = ref('')
const phoneErrorMessage = ref('')
const requirementsErrorMessage = ref('')
const isLoading = ref(false)
const isSuccess = ref(false)
const isError = ref(false)
const errorMessage = ref('')
const loggedInUserName = ref('')
const loggedInUserEmail = ref('')

const showNameField = computed(() => props.modalType === 'schedule-demo' || props.modalType === 'contact-us'|| props.modalType === 'generic-enquiry');

const successMessage = ref('');

const successAlt = computed(() => 
  props.modalType === 'forgot-password' ? 'Mail Sent' : 'Request demo mail sent'
)

const isValid = computed(() => 
  email.value.trim() && 
  (!showNameField.value || name.value.trim()) && 
  (!showNameField.value || phoneNumber.value.trim()) && 
  !invalidEmail.value && 
  (!showNameField.value || !invalidName.value) &&
  (!showNameField.value || !invalidPhone.value) &&
  (!showNameField.value || !invalidRequirements.value)
)

watch(email, (newEmail) => {
  if (newEmail.trim()) {
    invalidEmail.value = false
  }
})

watch(() => props.isVisible, () => {
  document.querySelector('html').style.overflow = props.isVisible ? 'hidden' : '';
});

watch(name, (newName) => {
  if (newName.trim()) {
    invalidName.value = false
  }
})

watch(phoneNumber, (newPhone) => {
  if (newPhone.trim()) {
    invalidPhone.value = false
  }
})

watch(requirements, (newRequirements) => {
  if (newRequirements.trim()) {
    invalidRequirements.value = false
  }
})

const handleClose = () => {
  email.value = ''
  name.value = ''
  phoneNumber.value = ''
  requirements.value = ''
  countryCode.value = '+1'
  invalidEmail.value = false
  invalidName.value = false
  invalidPhone.value = false
  invalidRequirements.value = false
  emailErrorMessage.value = ''
  nameErrorMessage.value = ''
  phoneErrorMessage.value = ''
  requirementsErrorMessage.value = ''
  isLoading.value = false
  isSuccess.value = false
  emit('close')
}

const validateName = () => {
  name.value = name.value.trim()
  
  if (!name.value) {
    nameErrorMessage.value = 'Name field cannot be left blank. Please enter your name.'
    invalidName.value = true
  } else if (name.value.length <= 2 || name.value.length > 50) {
    nameErrorMessage.value = 'Name must be between 2 and 50 characters long.'
    invalidName.value = true
  } else if (!/^[a-zA-Z\s]+$/.test(name.value)) {
    nameErrorMessage.value = 'Name can only contain letters and spaces.'
    invalidName.value = true
  } else {
    nameErrorMessage.value = ''
    invalidName.value = false
  }
}


function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
  
}

const validateEmail = () => {
  email.value = email.value.trim()
  const emailPattern = 
    /^[a-zA-Z0-9][\w.-]*[a-zA-Z0-9]@[a-zA-Z0-9]+[\w.-]*[a-zA-Z0-9]\.[a-zA-Z]{2,}$/
  const disposableEmailProviders = [
    'yopmail.com', 'mailinator.com', 'guerrillamail.com', 
    '10minutemail.com', 'aol.com', 'example.com', 
    'a.com', 'test.com'
  ]
  const [localPart, domain] = email.value.split('@')

  if (!email.value) {
    emailErrorMessage.value = 'Email field cannot be left blank. Please enter your email.'
    invalidEmail.value = true
  } else if (localPart.length < 6 || localPart.length > 30) {
    emailErrorMessage.value = 'The username must be between 6 and 30 characters long.'
    invalidEmail.value = true
  } else if (!emailPattern.test(email.value)) {
    emailErrorMessage.value = 'Please enter a valid email address.'
    invalidEmail.value = true
  } else if (disposableEmailProviders.includes(domain.toLowerCase())) {
    emailErrorMessage.value = 'The email domain is not allowed. Please use a valid provider.'
    invalidEmail.value = true
  } else {
    emailErrorMessage.value = ''
    invalidEmail.value = false
  }
}

const validatePhoneNumber = () => {
    phoneNumber.value = phoneNumber.value.trim()
      if(!phoneNumber.value || phoneNumber.value === ''){
        invalidPhone.value = true
        phoneErrorMessage.value  = "Phone Number is required.";
        return;
      }
      const phoneRegex = /^[+\d]+$/;
      if (!phoneRegex.test(phoneNumber.value)) {
        invalidPhone.value = true
        phoneErrorMessage.value = 'Phone Number contains invalid characters.';
      return; 
      }

      if (phoneNumber.value.length < 5) {
        invalidPhone.value = true;
        phoneErrorMessage.value = "Phone number is too short.";
        return;
      }

      const phoneNumberWithCountryCode = countryCode.value + phoneNumber.value;

      const phoneUtil = PhoneNumberUtil.getInstance();
      const parsedNumber = phoneUtil.parse(phoneNumberWithCountryCode);
      if (!phoneUtil.isValidNumber(parsedNumber)) {
        invalidPhone.value = true
        phoneErrorMessage.value = "Please enter a valid phone number for the selected country.";
      }
      else {
          invalidPhone.value = false
          phoneErrorMessage.value = "";
      }

      return;
  
}

const validateRequirements = () => {
  requirements.value = requirements.value.trim();

  if (requirements.value && (requirements.value.length < 10 || requirements.value.length > 500)) {
    requirementsErrorMessage.value = 'Requirements must be between 10 and 500 characters long.';
    invalidRequirements.value = true;
  } else {
    requirementsErrorMessage.value = '';
    invalidRequirements.value = false;
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
            'forgot-password': 'forgetpassword',
            'schedule-demo': 'requestdemo',
            'contact-us': 'contactUsForSubscription',
            'generic-enquiry': 'generic-enquiry'
        };

        const payloadMappings = {
            'forgot-password': () => ({
                Email: email.value,
            }),
            'schedule-demo': () => ({
                Name: name.value,
                Email: email.value,
                CountryCode: countryCode.value,
                Phone: phoneNumber.value,
                Requirements: requirements.value,
            }),
            'contact-us': () => ({
                Name: name.value,
                Email: email.value,
                CountryCode: countryCode.value,
                Phone: phoneNumber.value,
                AdditionalNotes: requirements.value,
                SelectedPlan: props.selectedPlan,
            }),
            'generic-enquiry': () => ({
                Name: name.value,
                Email: email.value,
                CountryCode: countryCode.value,
                Phone: phoneNumber.value,
                Requirements: requirements.value,
            }),
        };

        return {
            apiEndpoint: `api/manage/${apiEndpoints[props.modalType]}`,
            payload: payloadMappings[props.modalType]?.() || {},
        };
    };

    const submitReport = () => {
        emit('submitReport', {
            email: email.value,
            name: name.value,
        });
    };

    const handleResponse = (response) => {
        if (response.status === 200 && response.data.success) {
            isSuccess.value = true;
            successMessage.value = response.data.message;
            resetFormFields();
        } else {
            isError.value = true;
            errorMessage.value = response.data.message || 'An error occurred. Please try again.';
        }
    };

    const submitForm = async () => {
        validateFormFields();

        if (props.modalType === 'report-send') {
            submitReport();
            return;
        }

        if (isFormValid()) {
            isLoading.value = true;

            try {
                await new Promise((resolve) => setTimeout(resolve, 2000));
                const { apiEndpoint, payload } = getApiEndpointAndPayload();

                const response = await axios.post(apiEndpoint, payload, {
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });

                handleResponse(response);
            } catch (error) {
                isError.value = true;
                errorMessage.value = error.message;
                console.error('Error processing request', error);
            } finally {
                isLoading.value = false;
            }
        }
    };

const resetFormFields = () => {
  email.value = '';
  name.value = '';
  phoneNumber.value = '';
  requirements.value = '';
};

onMounted(() => {
  loggedInUserName.value = getCookie("firstName");
  loggedInUserEmail.value = getCookie("email");
  if(props.modalType === 'contact-us' && loggedInUserName.value  && loggedInUserEmail.value){
    name.value = loggedInUserName.value;
    email.value= loggedInUserEmail.value;
  }
});
</script>

<style scoped>
.modal {
  position: fixed;
  z-index: 999999;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(22, 21, 21, 0.767);
}

.modal-content {
  width: 450px;
  height: auto;
  top: 175px;
  left: 0;
  right: 0;
  margin-inline: auto;
  padding: 40px 40px 40px 40px;
  gap: 30px;
  border-radius: 17px;
  background: #ffffff;
  flex-direction: column;
  position: absolute;
}

.modal-content-schedule-demo {
  top: 5px;
  width: 450px;
  height: auto;
  left: 0;
  right: 0;
  margin-inline: auto;
  padding: 40px 40px 40px 40px;
  gap: 30px;
  border-radius: 17px;
  background: #ffffff;
  flex-direction: column;
  position: absolute;

  @media (max-width: 480px) {
    width: auto;
  }
} 

.hidden {
  overflow: hidden;
}

.mail-sussess {
  display: flex;
  flex-direction: column;
  align-items: center;
  height: 100%;
  width: 100%;
  padding-top: 20px;
}

.mail-sent {
  height: 179px;
  width: auto;
  margin-bottom: 20px; 
}

.success-message {
  width: 440px;
  text-align: center;
}

@media screen and (max-width: 480px) {
  .mail-sent {
    height: 200px;
  }
  
  .success-message {
    width: 90%;
    font-size: 14px;
  }
}

.close {
  width: 32px;
  height: 32px;
  padding: 8px;
  top: 10px;
  position: absolute;
  right: 10px;
  font-size: 35px;
  cursor: pointer;
  color: #aaa;
}

.close:hover {
  color: black;
}

.heading {
  height: 45px;
  margin: 0px;
  font-size :34px;
}

.content {
  width: 440px;
  height: 48px;
}

.requirements-input {
  padding: 16px 12px;
  border-radius: 4px;
  background: #ffffff;
  border: 1px solid #b6b4b2;
  resize: vertical;
  min-height: 50px;
  max-height: 200px;
}

button {
  position: relative;
  top: 15px;
  width: 100%;
  height: 48px;
  padding: 8px 20px;
  border-radius: 8px;
  background: #0066d4;
  color: white;
  border: none;
  margin-bottom: 20px;
}

button:hover {
  background-color: #005bb5;
  cursor: pointer;
}

.error-message {
  color: red;
  font-size: 12px;
  display: block;
}

.error {
  padding-bottom: 1px;
  border-color: red;
}

.input-group {
  display: flex;
  flex-direction: column;
}

.input-group label {
  font-size: 16px;
  font-weight: 400;
  color: #6d6d6d;
  position: relative;
  bottom: 5px;
  flex-direction: row;
}

.input-group input {
  height: 20px;
  padding: 16px 12px;
  border-radius: 4px;
  background: #ffffff;
  border: 1px solid #b6b4b2;
}

.phone-container {
  display: flex;
  gap: 10px;
}

.country-code {
  width: fit-content;
  padding: 17px 10px;
  border-radius: 4px;
  border: 1px solid #b6b4b2;
}

.phone-number {
  flex-grow: 1;
  width: 300px;
  height: 20px;
  padding: 16px 12px;
}

.phone-number.input {
  width: 100%;
}

.required {
  color: #FD1414;
  font-size: 14px;
  font-style: normal;
  font-weight: 700;
  line-height: 150%;
}

/* Responsive adjustments */
@media screen and (max-width: 720px) {
  .phone-container {
    flex-direction: row;
  }
}

button:disabled {
  background: #005bb5;
  cursor: not-allowed;
  opacity: 0.7;
}

button:disabled:hover {
  background-color: #0066d4;
  opacity: 0.7;
}


@media screen and (max-width: 1020px) {
  .modal-content {
    left: 20%;

  }
}

@media screen and (max-width: 720px) {
  .modal-content {
    left: 9.5%;
    top: 250px;
    max-width: 60%;

  }

  .content{
    width: 100%;
    margin-bottom: 20px;
  }

  .submit-button {
    width: 100%;
  }

  .heading{
    height: 40px;
  }
}
@media screen and (max-width: 420px) {
    .modal-content{
      left: 5.5%;
        top: 20%;
        max-width: 70%;
        padding: 10%;
    }
    .heading{
      margin-bottom: 0;
      font-size: 21px;
    }
    .content{
      margin-top: 0;
      font-size: 15px;
    }

    .input-group input { 
      height: 8px;
    }

    .submit-button{
      height: 35px;
      width: 100%;
    }
    .close{
    width: 28px;
    height: 28px;
    padding: 0px;
    top: 0px;
    position: absolute;
    right: 8px;
    font-size: 35px;
    cursor: pointer;
    color: #aaa;
}


}

.mb-8 {
  margin-bottom: 1rem;
}
</style>
