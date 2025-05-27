<template>
  <div class="form-container">
    <h2>Hospital/Clinic Information</h2>
    <form @submit.prevent="handleSave">
      <div class="form-left">
        <div class="form-group">
          <label for="hospitalName">Hospital/Clinic Name<span class="required">*</span></label>
          <input type="text" v-model="hospitalName" id="hospitalName" 
          @blur="validateField('hospitalName')" @input="clearError('hospitalName')"/>
          <span class="error">{{ errors.hospitalName }}</span>
        </div>

        <div class="form-group">
          <label for="address">Address<span class="required">*</span></label>
          <textarea placeholder="Enter address" v-model="address" id="address"
          maxlength="200" @blur="validateField('address')" @input="clearError('address')"/>
          <span class="error">{{ errors.address }}</span>
          <span class="char-count">{{ address.length }}/200</span>
        </div>
       
        <div class="form-group">
          <label for="phoneNumber">Phone Number<span class="required">*</span></label>
          <div class="phone-container">
            <select 
              v-model="countryCode" 
              class="input-group input country-code" 
              :class="{ error: invalidPhone }" @blur = "validateField('phoneNumber')"
            >
              <option value="+1">+1 (US)</option>
              <option value="+44">+44 (UK)</option>
              <option value="+91">+91 (IND)</option>
              <option value="+971">+971 (UAE)</option>
            </select>
            <div class="phone-form">
              <input class = "input-phone-number" type="text" v-model="phoneNumber" id="phoneNumber"
              @blur="validateField('phoneNumber')" @input="clearError('phoneNumber')"/>
              <span  class="error">{{ errors.phoneNumber }}</span>
            </div>
          </div>
          
        </div>

        <div class="form-group">
          <label for ="email">Email <span class="optional">(Optional)</span></label>
          <input type="text"  v-model="email" id="email"
           @blur="validateEmail(email)" @input="clearError('email')"/>
          <span  class="error">{{ errors.email }}</span>
        </div>

        <div class="form-group">
          <label for ="websiteLink">Website Link <span class="optional">(Optional)</span></label>
          <input type="text" v-model="websiteLink" id="websiteLink"/>
          <small>Please enter full path e.g., http:// or https:// before the URL, or the UNC path.</small>
        </div>
      </div>

      <div class="form-right">
        <label for ="iconImage">Hospital/Clinic Logo<span class="required">*</span></label>
        <diV v-if="iconImage" class="delete-icon" @click="deleteLogoImage">
            <img src="@/assets/icons/image-close-icon.svg" alt="Delete Icon" class="trashIcon" />
        </diV>
        <div>
            <div class="upload-box" 
                @dragover.prevent
                @dragenter.prevent
                @drop.prevent="handleFileDrop"
                >
                  <input 
                   type="file" 
                    ref="fileInput"
                    class="hiddenFileInput"
                    accept="image/jpeg, image/png"
                    @change="handleFileSelect" />

                    <div class="upload-icon" v-if="!iconImage">
                        <img src="@/assets/sign-upload-icon.png" alt="Upload Icon" />
                    </div>
                    <div class="dragAndDrop" v-if="!iconImage">
                        Drag & drop files or <span class="browseText" @click="browseFile">Browse</span>
                    </div>
                    <div class="supportedText" v-if="!iconImage">
                        Supported formates: JPEG, PNG
                    </div>
                    <div v-if="iconImage" class="imagePreview">
                        <img :src="iconImage" alt="Uploaded Signature Preview" class="previewImage"/>
                    </div>
            </div>
          <span class="error-image-upload" v-if="isDocIconError">{{ iconError }}</span>
        </div>
      </div>

      <div class="button-group">
        <button type="button" class="cancel-btn" @click="cancelForm">Cancel</button>
        <button type="submit" class="save-btn">Save</button>
      </div>
    </form>
  </div>
</template>

<script setup>
import axiosInstance from '@/Services/Interceptors/axios.js';
import { PhoneNumberUtil } from 'google-libphonenumber';
import { onBeforeUnmount, onMounted, ref } from 'vue';

const hospitalName = ref("");
const countryCode = ref('+1');
const invalidPhone = ref(false);
const phoneNumber = ref("");
const email = ref("");
const websiteLink = ref("");
const address = ref("");
const docIcon = ref(null);
const iconImage = ref("");
const isDocIconError = ref(false);
const iconError = ref("");
const fileInput = ref(null);
const imagePreview = ref(null);
const userId = ref("");
const success = ref(false);
const message = ref("");
const logo = ref("");
const errors = ref({
    hospitalName: '',
    phoneNumber: '',
    email: '',
    address: '',
});

function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
  return null;
}
// Clean up the preview URL to avoid memory leaks
onBeforeUnmount(() => {
  if (imagePreview.value) {
    URL.revokeObjectURL(imagePreview.value);
  }
});
onMounted(() => {
  userId.value = getCookie("userId");
  getClinicDetails(userId.value);
})

const validateEmail = (email) => {
    trimField("email");
    const emailPattern = /^[a-zA-Z0-9][\w.-]*[a-zA-Z0-9]@[a-zA-Z0-9]+[\w.-]*[a-zA-Z0-9]\.[a-zA-Z]{2,}$/;
    const disposableEmailProviders = [
        'yopmail.com', 'mailinator.com', 'guerrillamail.com', '10minutemail.com', 
        'aol.com', 'example.com', 'a.com', 'test.com'
    ];
    const trimmedEmail = email.trim();
    const domain = trimmedEmail.split('@')[1];

    if (email === '' || email.trim() === '') {
      errors.value.email = '';
    }else if (!emailPattern.test(trimmedEmail)) {
      errors.value.email = 'Please enter a valid email address.'; 
    }else if (domain && disposableEmailProviders.includes(domain.toLowerCase())) {
      errors.value.email = 'The email domain you have entered is not allowed. Please use a valid email provider.' ;
    }
};

const trimField = (field) => {
    errors.value[field] = "";
    switch (field) {
      case "hospitalName": {
        if (hospitalName.value) {
          hospitalName.value = hospitalName.value.trim();
        }
        break;
      }
      case "email": {
        if (email.value) {
          email.value = email.value.trim();
        }
        break;
      }
      case "phoneNumber": {
        if (phoneNumber.value) {
          phoneNumber.value = phoneNumber.value.trim();
        }
        break;
      }
      case "address": {
        if (address.value) {
          address.value = address.value.trim();
        }
        break;
      }
      case "websiteLink": {
        if (websiteLink.value) {
          websiteLink.value = websiteLink.value.trim();
        }
        break;
      }
      
    }
  };

const validateField = (field) => {
  errors.value[field] = "";
  trimField(field);
  switch (field) {
    case "hospitalName":{
      if (!hospitalName.value) {
        errors.value.hospitalName = "Hospital/Clinic Name is required.";
      } else if (hospitalName.value.length < 3) {
        errors.value.hospitalName = "Hospital/Clinic Name must be at least 3 characters long.";
      } else if (hospitalName.value.length > 200) {
        errors.value.hospitalName = "Hospital/Clinic Name must not exceed 200 characters.";
      } else {
          const validNameRegex = /^[A-Za-z\s'-.&]+$/;

        if (!validNameRegex.test(hospitalName.value)) {
          errors.value.hospitalName =
            "Please Enter Valid Hospital/Clinic Name.";
        }
      }
      break;
    }
    case "phoneNumber": {
      phoneNumber.value = phoneNumber.value.trim()
      if(!phoneNumber.value || phoneNumber.value === ''){
        errors.value.phoneNumber = "Phone Number is required.";
        break;
      }
      const phoneRegex = /^[+\d]+$/;
      if (!phoneRegex.test(phoneNumber.value)) {
       errors.value.phoneNumber = 'Phone Number contains invalid characters.';
       break;
      }

      if (phoneNumber.value.length < 5) {
        errors.value.phoneNumber = "Phone number is too short.";
        return;
      }

      const phoneNumberWithCountryCode = countryCode.value + phoneNumber.value;

      const phoneUtil = PhoneNumberUtil.getInstance();
      const parsedNumber = phoneUtil.parse(phoneNumberWithCountryCode);
      if (!phoneUtil.isValidNumber(parsedNumber)) {
          errors.value.phoneNumber = "Please enter a valid phone number for the selected country.";
      }
      else {
          errors.value.phoneNumber = "";
      }

      break;
    }
    case "address": {
      if (!address.value) {
        errors.value.address = "Address is required.";
      } else if (address.value.length < 3) {
        errors.value.address = "Address must be at least 3 characters long.";
      }
      break;
    }
  }
};
const browseFile = () => {
    document.querySelector(".hiddenFileInput").click();
};
const handleFileSelect = (event) => {
    const file = event.target.files[0];
    validateAndProcessFile(file);
    event.target.value = "";
};

const handleFileDrop = (event) => {
    const file = event.dataTransfer.files[0];
    validateAndProcessFile(file);
    event.dataTransfer.value = "";
};

const validateAndProcessFile = (file) => {

  if (file) {
    const fileExtension = file.name.split(".").pop().toLowerCase();
    if (fileExtension === "png" || fileExtension === "jpg") {
      const reader = new FileReader();
      reader.onload = (e) => {
        iconImage.value = e.target.result;
        docIcon.value = e.target.result;
        isDocIconError.value = false;
        iconError.value = "";
      };
      reader.readAsDataURL(file);
    } else {
        iconError.value = "Please upload a valid file format";
        isDocIconError.value = true;
        setTimeout(() => {
        isDocIconError.value = false;
        iconError.value = "";
    }, 3000);
    }
  }
};

const deleteLogoImage = () => {
    iconImage.value = "";  
    docIcon.value = "";    
};
const clearError = (field) => {
  errors.value[field] = "";
};

const handleSave = async () => {
  validateField("hospitalName");
  validateField("phoneNumber");
  validateField("address");

  if (!docIcon.value) {
    iconError.value = "Please upload the Logo";
    isDocIconError.value = true; 
    return; 
  }
  const hasErrors =
    Object.values(errors.value).some((error) => error !== "");
  if (hasErrors) {
    return;
  }
  const payload = {
    UserId: userId.value,
    ClinicName: hospitalName.value,
    ClinicAddress: address.value,
    PhoneNumber: phoneNumber.value,
    CountryCode: countryCode.value,
    Email: email.value,
    Website: websiteLink.value,
    Logo: docIcon.value,
  };
  try {
    
    const response = await axiosInstance.post("/api/settings/update-clinic", payload);
    if (response.data.success) {
      message.value = response.data.message;
      success.value = response.data.success;
      window.dispatchEvent(new CustomEvent("settings-updated", {
        detail: {
          success: true,
          message: "Clinic information updated successfully."
        }
      }));
      scrollToTop();
    }else {
      window.dispatchEvent(new CustomEvent("settings-updated", {
        detail: {
          success: false,
          message: "Clinic information update failed."
        }
      }));
    }
  }
  catch (err) {
    message.value = "An error occurred while updating the clinic information.";
    success.value = false;

    window.dispatchEvent(
      new CustomEvent("toast-event", {
        detail: {
          success: success.value,
          message: message.value,
        },
      })
    );
  }
};

const getClinicDetails = async (userId) => {

  try {

    const response = await axiosInstance.get(`/api/settings/get-clinic?userId=${userId}`);
    
    hospitalName.value = response.data.clinicName;
    address.value = response.data.clinicAddress;
    phoneNumber.value = response.data.phoneNumber;
    countryCode.value = response.data.countryCode;
    email.value = response.data.email;
    websiteLink.value = response.data.website;

    let imageType = "png";
    if (response.data.logo != null) {
      if (response.data.logo.startsWith("/9j")) {
        imageType = "jpeg";
      }
      logo.value = `data:image/${imageType};base64,${response.data.logo}`;
      iconImage.value = logo.value;
      docIcon.value = logo.value;
    } else {
      iconImage.value = "";
      docIcon.value = "";
    }
    
  } catch (err) {
    console.error("An error occurred while fetching user data.");
  }
};

const cancelForm = () => {
  errors.value.hospitalName = "";
  errors.value.phoneNumber = "";
  errors.value.address = "";
  isDocIconError.value = false;
  iconError.value = "";
  scrollToTop();
  getClinicDetails(userId.value);
};

const scrollToTop = () => {
  window.scrollTo({
    top: 0,
    behavior: 'smooth'
  })
};
</script>

<style scoped>
.form-container {
 display: flex;
  flex-direction: column;
  align-items: flex-start;
  gap: 9px;
  flex-shrink: 0;
  border-radius: 12px;
} 

h2 {
  color: #1C1C1C;
  font-size: 22px;
  font-style: normal;
  font-weight: 600;
  line-height: normal;
  margin-top: 1px;
}

form {
  display: flex;
  gap: 40px;
}

.form-right {
  flex: 1;
  gap: 30px;
  margin-left: 268px;
  
}

.form-group {
  margin-bottom: 21px;
}

label {
  color: var(--Text-light, #5A5A5A);
  font-size: 14px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%; 
}

input[type="text"]{
  display: flex;
  height: 10px;
  padding: 16px 16px 16px 12px;
  align-items: center;
  gap: 8px;
  align-self: stretch;
  border: 1px solid #EEE;
  border-radius: 4px;
  background-color: var(--Background-White, #FFF);
  width: 286px;
}
textarea {
  display: flex;
  padding: 16px 16px 16px 12px;
  align-items: center;
  gap: 8px;
  flex: 1 0 0;
  align-self: stretch;
  border-radius: 4px;
  border: 1px solid #EEE;
  background: var(--Background-White, #FFF);
  width: 353px;
  height: 100px;
  
}

.input-phone-number{
  width: 234px !important;
}

textarea::placeholder {
 color: #C3C3C3;
 font-size: 14px;
 font-style: normal;
 font-weight: 400;
 line-height: 150%; /* 21px */
 resize: vertical;
 }
.upload-box {
 width: 231px;
  height: 212px;
  flex-shrink: 0;
  border-radius: 3.663px;
  border: 0.916px dashed rgba(0, 102, 212, 0.30);
  background: #EEF6FF;
  overflow: hidden;
  position: relative;
}
.optional {
  color: var(--Text-light, #5A5A5A);
  font-size: 14px;
  font-style: italic;
  font-weight: 400;
  line-height: 150%;
}

.required {
  color: #FD1414;
  font-size: 14px;
  font-style: normal;
  font-weight: 700;
  line-height: 150%;
}

.button-group {
  display: flex;
  justify-content: space-between;
  width: 135%;
  margin-top: 581px;
  gap: 803px;
  margin-left: -959px;
}

.cancel-btn {
  display: flex;
  height: 42px;
  padding: 8px 20px;
  justify-content: center;
  align-items: center;
  gap: 8px;
  flex-shrink: 0;
  border-radius: 8px;
  border: 1px solid #eee;
  background: var(--bg-white, #fff);
  cursor: pointer;
}

.save-btn {
  display: flex;
  height: 42px;
  padding: 8px 20px;
  justify-content: center;
  align-items: center;
  gap: 8px;
  flex-shrink: 0;
  border-radius: 8px;
  background: var(--Primary-Colors, #0066d4);
  cursor: pointer;
  border: none;
  color: var(--Text-Text-white, #fff);
  
}
small{
  color: var(--Colors-Text-Colors-Text-Label, #6D6D6D);
  font-size: 11px;
  font-style: italic;
  font-weight: 400;
  line-height: 150%; /* 18px */
}

.upload-icon{
  align-items: center;
  width: 64px;
  height: 55px;
  display: flex;
  margin-top: 39px;
  margin-left: 84px;
}

.error-image-upload,
.error{
  color: red;
  font-size: 12px;
  position: absolute;
}

.char-count{
  font-size: 11px;
  margin-left: 338px;
}

.hiddenFileInput {
    display: none;
}
.trashIcon{
  width: 89px;
  height: 20px;
}
.delete-icon {
  position: absolute;
  top: 240px;
  left: 1360px;
  cursor: pointer;
}
.dragAndDrop{
    padding: 8.96px;
    color: #0F0F0F;
    font-size: 12.673px;
    font-weight: 700;
    margin-left: 20px;
    margin-top: 7px;
}
.supportedText{
    padding: 3.96px;
    color: #676767;
    font-size: 10.505px;
    font-weight: 400;
    line-height: 14.257px;
    margin-left: 29px;
}
.imagePreview {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100%;
}
.previewImage {
    border-radius: 3px;
    width: 231px;
    height: 212px;
    object-fit: cover;
}
.phone-container {
  display: flex;
  border-radius: 4px;
  border: 1px solid var(--Border-Primary-100, #efefef);
  overflow: hidden;
}

.phone-container input {
  flex: 1;
  border: none;
  border-radius: 4px;
  padding: 16px 18px;
}
.country-code {
  border: none;
  border-radius: 4px;
  padding: 13px 12px;
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
.browseText{
    color: #0066D4;
    font-size: 12.673px;
    font-weight: 700;
    line-height: 19.01px;
    text-decoration-line: underline;
    text-decoration-style: solid;
    text-decoration-skip-ink: none;
    text-decoration-thickness: auto;
    text-underline-offset: auto;
    text-underline-position: from-font;
    cursor: pointer;
}
</style>
