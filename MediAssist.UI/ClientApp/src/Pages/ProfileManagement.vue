<template>
  <div class="container">
    <SideBar />
    <div class="header-section">
      <div user-menu>
        <img
          :src="image"
          alt="Doctor Icon"
          class="doctor-icon"
          @click="toggleMenu"
        />
      </div>
      <div class="user-profile">
        <ProfileDropDown :menuOpen="menuOpen" />
      </div>
    </div>
    <div class="profile-management">
      <div class="my-profile-title">
        <h2>My Profile</h2>
        <div class="toast" v-if="showToast">
          <div
            class="profile-update"
            :class="{ success: success, errorUpdate: !success }"
          >
            <img
              v-if="success"
              class="bold-essentional-ui-check"
              alt="success icon"
              src="../assets/check-circle.png"
            />
            <img
              v-else
              class="bold-essentional-ui-error"
              alt="error icon"
              src="../assets/close-circle.png"
            />
            <div class="profile-updated-successfully">
              {{ message }}
            </div>
            <button class="icon" @click="showToast = false">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="11"
                height="11"
                viewBox="0 0 11 11"
                fill="none"
              >
                <path
                  d="M1 9.75L10 1.25M1 1.25L10 9.75"
                  stroke="#7C7C7C"
                  stroke-width="1.2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                />
              </svg>
            </button>
          </div>
        </div>
      </div>
      <div class="profile-container">
        <div class="profile-card">
          <div class="profile-top"></div>
          <div class="profile-info"></div>
          <div class="profile-content">
            <img
              v-if="profileImage"
              :src="profileImage"
              alt="Profile"
              class="profile-image"
            />
            <div v-else class="placeholder-image"></div>
            <label class="edit-icon">
              <input
                type="file"
                @change="onImageUpload"
                accept="image/*"
                style="display: none"
              />
              <i class="edit">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="17"
                  height="17"
                  viewBox="0 0 17 17"
                  fill="none"
                >
                  <path
                    fill-rule="evenodd"
                    clip-rule="evenodd"
                    d="M10.0738 2.19738C11.2928 0.978315 13.2693 0.978315 14.4884 2.19738C15.7074 3.41644 15.7074 5.39294 14.4884 6.612L8.16485 12.9355C7.80357 13.2968 7.5909 13.5095 7.3543 13.6941C7.07544 13.9116 6.7737 14.0981 6.45444 14.2502C6.18358 14.3793 5.89823 14.4744 5.41349 14.636L3.19274 15.3762L2.65802 15.5545C2.22432 15.699 1.74615 15.5861 1.42288 15.2629C1.09962 14.9396 0.986737 14.4614 1.13131 14.0277L2.04979 11.2723C2.21135 10.7875 2.30646 10.5022 2.43555 10.2313C2.5877 9.91206 2.77418 9.61033 2.99169 9.33146C3.17623 9.09486 3.38893 8.88218 3.75025 8.52089L10.0738 2.19738ZM3.16968 14.3298L5.06402 13.6984C5.59143 13.5226 5.81552 13.447 6.02422 13.3475C6.27792 13.2266 6.51768 13.0784 6.73928 12.9056C6.92158 12.7634 7.08942 12.5968 7.48253 12.2036L12.5288 7.15732C12.0031 6.97189 11.3258 6.62817 10.6917 5.99409C10.0576 5.36 9.71387 4.68267 9.52844 4.15691L4.48212 9.20323C4.08901 9.59634 3.92239 9.76418 3.7802 9.94648C3.60736 10.1681 3.45918 10.4078 3.33827 10.6615C3.23881 10.8702 3.1632 11.0943 2.9874 11.6217L2.35595 13.5161L3.16968 14.3298ZM10.3396 3.34578C10.3624 3.46242 10.4009 3.62098 10.4652 3.80633C10.61 4.22363 10.8835 4.77167 11.3988 5.28698C11.9141 5.80229 12.4621 6.07577 12.8794 6.22055C13.0648 6.28485 13.2233 6.32339 13.34 6.34619L13.7813 5.9049C14.6098 5.07636 14.6098 3.73303 13.7813 2.90449C12.9527 2.07595 11.6094 2.07595 10.7809 2.90449L10.3396 3.34578Z"
                    fill="#707070"
                  />
                </svg>
              </i>
            </label>

            <h3 :title="fullName">{{ fullName }}</h3>
            <p :title="email">{{ email }}</p>
          </div>
          <span class="error-image-upload" v-if="isProfileImageError">Please upload a valid profile picture.</span>
        </div>
        <div class="profile-form">
          <form class="form-container" @submit.prevent="handleSave">
            <div class="personal-details-container">            
            <div class="pers-details-title">
              <h3>Personal Details</h3>
            </div>
            <div class="tit-and-fullName">
              <div class="form-group">
                <label for="title">Title</label>
                <select
                  name="title"
                  id="title"
                  v-model.number="title"
                  @blur="validateField('title')"
                >
                  <option :value="1">Dr. (Doctor)</option>
                  <option :value="2">Consultant</option>
                  <option :value="3">Resident</option>
                  <option :value="4">Attending Physician</option>
                  <option :value="5">Senior Consultant</option>
                  <option :value="6">Chief Surgeon</option>
                  <option :value="7">Clinical Lead</option>
                </select>
                <span v-if="errors.title" class="error">{{
                  errors.title
                }}</span>
              </div>
              <div class="form-group">
                <label for="fullName">Full Name</label>
                <input
                  type="text"
                  name="fullName"
                  id="fullName"
                  v-model="fullName"
                  disabled
                />
              </div>
            </div>
            <div class="gen-and-dob">
              <div class="form-group">
                <label for="gender">Gender</label>
                <select
                  name="gender"
                  id="gender"
                  v-model.number="gender"
                  @blur="validateField('gender')"
                >
                  <option :value="1">Male</option>
                  <option :value="2">Female</option>
                  <option :value="3">Transgender</option>
                  <option :value="4">Non-binary</option>
                  <option :value="5">Prefer Not to Say</option>
                </select>
                <span v-if="errors.gender" class="error">
                  {{ errors.gender }}
                </span>
              </div>
              <div class="form-group">
                <label for="dob">Date Of Birth</label>
                <input
                  type="date"
                  name="dob"
                  id="dob"
                  v-model="dob"
                  class="dob-picker"
                  @blur="validateDob(dob)"
                  @input="validateDob(dob)"
                  :max="getCurrentDate()"
                />
                <span v-if="dobErrors.blankDob" class="error"
                  >Date of Birth cannot be left blank.</span
                >
                <span v-if="dobErrors.invalidDateFormat" class="error"
                  >Please enter a valid date in the format.</span
                >
                <span v-if="dobErrors.invalidAge" class="error"
                  >The age must be above 18 years.</span
                >
                <span v-if="dobErrors.futureDate" class="error"
                  >Date of birth cannot be a future date.</span
                >
              </div>
            </div>
          </div>
            <div class="prof-details-title">
              <h3>Professional Details</h3>
            </div>
            <div class="med-and-spec">
              <div class="form-group">
                <label for="lic-reg-no">License/Registration number</label>
                <input
                  type="text"
                  name=""
                  id="lic-reg-no"
                  v-model="licRegNo"
                  @input="validateLicenseReg"
                />
              </div>
              <div class="form-group">
                <label for="medCred">Medical Credentials</label>
                <select
                  name="medCred"
                  id="medCred"
                  v-model.number="medCred"
                  @blur="validateField('medCred')"
                >
                  <option :value="1" class="try-Width">MD (Doctor of Medicine)</option>
                  <option :value="2" class="try-Width">
                    MBBS (Bachelor of Medicine, Bachelor of Surgery)
                  </option>
                  <option :value="3" class="try-Width">
                    DO (Doctor of Osteopathic Medicine)
                  </option>
                  <option :value="4" class="try-Width">BDS (Bachelor of Dental Surgery)</option>
                  <option :value="5"  class="try-Width">MCh (Master of Surgery)</option>
                  <option :value="6" class="try-Width">DM (Doctorate of Medicine)</option>
                  <option :value="7" class="try-Width">
                    FRCS (Fellowship of the Royal College of Surgeons)
                  </option>
                  <option :value="8" class="try-Width">
                    FACP (Fellow of the American College of Physicians)
                  </option>
                  <option :value="9" class="try-Width">MS (Master of Surgery)</option>
                  <option :value="10" class="try-Width">DNB (Diplomate of National Board)</option>
                </select>
                <span v-if="errors.medCred" class="error">
                  {{ errors.medCred }}
                </span>
              </div>
              <div class="form-group">
                <label for="specialization">Specialization</label>
                <input
                  type="text"
                  name="specialization"
                  id="specialization"
                  v-model="specialization"
                  @blur="validateField('specialization')"
                  @input="validateSpecialization"
                />
                <span v-if="errors.specialization" class="error">
                  {{ errors.specialization }}
                </span>
              </div>
            </div>
            <div class="button-group">
              <button
                type="button"
                @click="deleteProfileAction"
                class="delete-btn"
              >
                Delete Account
              </button>
              <div class="second-btn-grp">
                <button
                type="button"
                @click="cancelProfileAction"
                class="cancel-btn">
                Cancel
              </button>
              <button type="submit" class="save-btn">Save</button>
              </div>              
            </div>
          </form>
        </div>
      </div>
    </div>

    <DeleteAccountAlert
      v-if="showDeletePopup"
      :isVisible="showDeletePopup"
      title="Confirm Account Deletion"
      message = "Are you sure you want to delete your account? This action is irreversible, and all your data will be permanently removed."
      confirmText="Confirm"
      cancelText="Cancel"
      @confirm="confirmDeleteAccount"
      @cancel="cancelDeleteAccount"
    />
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from "vue";
import SideBar from "../components/SideBar.vue";
import { useRouter } from 'vue-router';
import DeleteAccountAlert from "@/components/alert/ConfirmationAlert.vue";
import Icon from "@/assets/doctor-icon.png";
import ProfileDropDown from "@/components/ProfileDropDown.vue";
import axiosInstance from '../Services/Interceptors/axios.js';

const router = useRouter();
const image = ref("");
const profileImage = ref(null);
const title = ref(0);
const fullName = ref("");
const email = ref("");
const gender = ref(0);
const dob = ref("");
const medCred = ref(0);
const specialization = ref("");
const licRegNo = ref("");
const userId = ref("");
const menuOpen = ref(false);
const showDeletePopup = ref(false);
const errors = ref({
  title: "",
  medCred: "",
  specialization: "",
  gender: "",
});
const dobErrors = ref({
  invalidDateFormat: false,
  invalidAge: false,
  futureDate: false,
  blankDob: false,
});
const profileUpload = ref(false);
const profileUploadMessage = ref("");
const message = ref("");
const success = ref(false);
const showToast = ref(false);
const isProfileImageError = ref(false);

const onImageUpload = (event) => {
  const file = event.target.files[0];

  if (file) {
    const fileExtension = file.name.split(".").pop().toLowerCase();
    if (fileExtension === "png" || fileExtension === "jpg") {
      const reader = new FileReader();
      reader.onload = (e) => {
        profileImage.value = e.target.result;
        profileUploadMessage.value = "Profile updated successfully.";
        isProfileImageError.value = false;
        profileUpload.value = true;
      };
      reader.readAsDataURL(file);
    } else {
      isProfileImageError.value = true;
      setTimeout(() => {
        isProfileImageError.value = false;
    }, 3000);
    }
  }
};


const handleSave = async () => {
  validateField("title");
  validateField("medCred");
  validateField("specialization");
  validateField("gender");
  validateDob(dob.value);
  const hasErrors =
    Object.values(errors.value).some((error) => error !== "") ||
    Object.values(dobErrors.value).some((error) => error === true);
  if (hasErrors) {
    return;
  }
  const payload = {
    Title: title.value,
    Gender: gender.value,
    DOB: new Date(dob.value),
    Image: profileImage.value,
    LicenseNumber: licRegNo.value,
    MedicalCredentials: medCred.value,
    Specialization: specialization.value,
    UserId: userId.value,
  };
  try {
    const response = await axiosInstance.post("/api/profile/update", payload);
      if (response.data.success) {
      document.cookie = `title=${encodeURIComponent(response.data.title)}; path=/; Secure; SameSite=Strict`;
      document.cookie = `specialization=${encodeURIComponent(specialization.value)}; path=/; Secure; SameSite=Strict`;
       if (profileImage.value) {
        const imageBase64 = profileImage.value.split(",")[1]; 
         localStorage.setItem("image", imageBase64);

        // Immediately update the displayed profile image after a successful upload
        let imageType = "png";
        if (profileImage.value.startsWith("/9j")) {
          imageType = "jpeg";
        }
        image.value = `data:image/${imageType};base64,${imageBase64}`;
      }

      window.dispatchEvent(new Event("profile-updated"));
    } else {
      console.error("Profile update failed");
    }
    message.value = response.data.message;
    success.value = response.data.success;
  } catch (error) {
    message.value = "An error occurred while updating the profile.";
    success.value = false;
  } finally {
    showToast.value = true;
    setTimeout(() => {
      showToast.value = false;
    }, 3000);
  }
};

const validateField = (field) => {
  errors.value[field] = "";
  switch (field) {
    case "title": {
      if (!title.value) {
        errors.value.title = "Title is a required field.";
      }
      break;
    }
    case "medCred": {
      if (!medCred.value) {
        errors.value.medCred =
          "Please provide at least one medical credential.";
      }
      break;
    }
    case "specialization": {
      if(specialization.value){
        specialization.value = specialization.value.trim();
      }
      else if (!specialization.value) {
        errors.value.specialization = "Specialization field cannot be empty.";
      }
      break;
    }
    case "gender": {
      if (!gender.value) {
        errors.value.gender = "Gender is a required field.";
      }
      break;
    }
  }
};

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
function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
  return null;
}

onMounted(() => {
  userId.value = getCookie("userId");
  const imageValue = localStorage.getItem("image");

   let imageType = "png";
     if (imageValue && imageValue !== "null") {
       if (imageValue.startsWith("/9j")) {
         imageType = "jpeg";
       }
       image.value = `data:image/${imageType};base64,${imageValue}`;
     }
     else{
       image.value = Icon;
     }
  getDocterDetails(userId.value);
  document.addEventListener("click", handleClickOutside);
});
onUnmounted(() => {
  document.removeEventListener("click", handleClickOutside);
});

const getDocterDetails = async (userId) => {
  try {

    const response = await axiosInstance.get(`/api/profile/get?UserId=${userId}`);

    let imageType = "png";
    if (response.data.image != null) {
      if (response.data.image.startsWith("/9j")) {
        imageType = "jpeg";
      }
      profileImage.value = `data:image/${imageType};base64,${response.data.image}`;
    } else {
      profileImage.value = "";
    }
    title.value = response.data.title;
    fullName.value = response.data.fullName;
    email.value = response.data.email;
    gender.value = response.data.gender;
    if (response.data.dob) {
      const dateFromApi = new Date(response.data.dob);
      dob.value = dateFromApi.toLocaleDateString("en-CA");
    } else {
      dob.value = "";
    }
    medCred.value = response.data.medicalCredentials;
    specialization.value = response.data.specialization;
    licRegNo.value = response.data.licenseNumber;
  } catch (err) {
    console.log("An error occurred while fetching user data.");
  }
};

const cancelProfileAction = () => {
  getDocterDetails(userId.value);
};
const toggleMenu = () => {
  menuOpen.value = !menuOpen.value;
};
const handleClickOutside = (event) => {
  const menu = document.querySelector(".user-profile");
  const userIcon = document.querySelector(".doctor-icon");
  if (
    menu &&
    !menu.contains(event.target) &&
    !userIcon.contains(event.target)
  ) {
    menuOpen.value = false;
  }
};
const validateLicenseReg = () => {
  const regex = /^[a-zA-Z0-9]*$/;
  if (!regex.test(licRegNo.value)) {
    licRegNo.value = licRegNo.value.replace(/[^a-zA-Z0-9]/g, "");
  }
};
const validateSpecialization = (event) => {
  const regex = /^[A-Za-z\s]*$/;
  const inputValue = event.target.value;
  if (regex.test(inputValue)) {
    specialization.value = inputValue;
  } else {
    const sanitizedValue = inputValue.replace(/[^A-Za-z\s]/g, "");
    specialization.value = sanitizedValue;
    event.target.value = sanitizedValue;
  }
};

const deleteProfileAction = () => {
  showDeletePopup.value = true;
  
};

const confirmDeleteAccount = () => {
  deleteUserAccout(userId.value);
};

const cancelDeleteAccount = () => {
  showDeletePopup.value = false;
};

window.addEventListener('storage', (event) => {
  if (event.key === 'accountDeleted') {
    redirectAndLockToRoot();
  }
});
const deleteUserAccout = async (userId) => {
  localStorage.setItem('accountDeleted', Date.now());
  try {
    const response = await axiosInstance.delete(`/api/profile/delete?UserId=${userId}`);

    if(response.data.success){
      await axiosInstance.post('/api/manage/logout');
      localStorage.clear();
      sessionStorage.clear();
         const clearCookie = (name) => {
          document.cookie = `${name}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT; SameSite=Strict`;
          };
          ['userId', 'firstName', 'specialization', 'title'].forEach(clearCookie);
           sessionStorage.setItem('showDeleteSuccessPopup', 'true');
             localStorage.setItem('userLogOut', Date.now());
            
      redirectAndLockToRoot();
              
      }else {
      showDeletePopup.value = false;
      showToast.value = true;
      setTimeout(() => {
      showToast.value = false;
      }, 5000);
      success.value = false;
      message.value = "An error occurred. Please try again.";
    }
  }
  catch (err) {
    showDeletePopup.value = false;
      showToast.value = true;
      setTimeout(() => {
      showToast.value = false;
      }, 5000);
      success.value = false;
      message.value = "An error occurred. Please try again.";
  }
};

function redirectAndLockToRoot() {
  router.replace('/'); 
  history.replaceState(null, null, '/');
  history.pushState(null, null, '/');

  window.onpopstate = function () {
    history.pushState(null, null, '/'); 
  };
}

window.addEventListener('delete-account', (event) => {
  if (event.key === 'userLogOut') {
    redirectAndLockToRoot();
  }
});
</script>

<style scoped>
.error-message {
  display: flex;
  align-items: center;
  gap: 8px;
  align-self: stretch;
  border-radius: 4px;
  background: #ffdfdd;
  position: absolute;
  top: 205px;
}
.error-message span {
  color: var(--Colors-Text-Colors-Text-Primary, #1c1c1c);
  font-size: 12px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%;
}

.bold-essentional-ui-check,
.bold-essentional-ui-error {
  width: 21px;
  height: 21px;
}

.profile-updated-successfully {
  position: relative;
  line-height: 150%;
  width: 87%;
  font-size: 12px;
  color: #1c1c1c;
  flex-grow: 1;
}
.icon {
  width: 19%;
    position: relative;
    height: 17.5px;
    border: none;
    background-color: transparent;
    cursor: pointer;
    margin-left: -61px
}
.profile-update.errorUpdate {
  background-color: #ffeaea;
}
.profile-update.success {
  background-color: #edf8ed;
}
.profile-update {
  display: inline-flex;
    flex-direction: row;
    align-items: center;
    justify-content: flex-start;
    gap: 7px;
    width: 89%;
    padding: 10px;
    border-radius: 4px
}

.toast {
  position: absolute;
  right: 34%;
  top: 14%;
  width: 22%;
}
.dob-picker[value]::-webkit-datetime-edit-fields-wrapper {
  color: black;
}
.dob-picker[value]::-webkit-datetime-edit-text,
.dob-picker[value]::-webkit-datetime-edit-month-field,
.dob-picker[value]::-webkit-datetime-edit-day-field,
.dob-picker[value]::-webkit-datetime-edit-year-field {
  color: black;
}
.dob-picker:focus::-webkit-datetime-edit {
  color: black;
}
.dob-picker::-webkit-datetime-edit-fields-wrapper {
  color: black;
}
.dob-picker::-webkit-datetime-edit {
  color: transparent;
}
.dob-picker::-webkit-datetime-edit-text {
  text-transform: lowercase;
  color: #757575;
}
.dob-picker::-webkit-datetime-edit-month-field {
  text-transform: lowercase;
  color: #757575;
}
.dob-picker::-webkit-datetime-edit-day-field {
  text-transform: lowercase;
  color: #757575;
  font-size: 2px;
}
.dob-picker::-webkit-datetime-edit-year-field {
  text-transform: lowercase;
  color: #757575;
}
@-moz-document url-prefix() {
  .dob-picker {
    color: #999;
  }

  .dob-picker[value] {
    color: #333;
  }
}

.profile-management {
  display: flex;
  flex-direction: column;
  background: #f3f9ff;
  margin-left: 80px;
  margin-top: 0;
  width: auto;
  height: auto;
}

.my-profile-title {
  display: flex;
  align-items: center;
  align-self: stretch;
  padding-left: 17px;
}

.my-profile-title h2 {
  color: #263238;
  font-size : 20px;
  font-style: normal;
  font-weight: 600;
  line-height: normal;
  letter-spacing: -0.2px;
}

.profile-container {
  display: flex;
  margin-top: 10px;
  gap: 16px;
  flex-wrap: wrap;
}

.profile-card {
  position: relative;
  margin-left: 3%;
  width: 212px;
  display: flex;
  flex-direction: column;
  height: 200px;
}

.profile-top {
  width: 212.001px;
  height: 116.484px;
  flex-shrink: 0;
  border-radius: 8px;
  background: #003482;
  display: flex;
  flex-direction: column;
  position: relative;
}

.profile-info {
  width: 210.001px;
  height: 130px;
  flex-shrink: 0;
  border-radius: 8px;
  border: 1px solid var(--Colors-Status-Colours-Primary-Main, #007aff);
  background: var(--bg-white, #fff);
  position: absolute;
  top: 65px;
}

.profile-form {
  display: flex;
  width:67%;
  height: auto;
  padding: 20px;
  flex-direction: column-reverse;
  align-items: flex-start;
  justify-content: normal;
  gap: 10px;
  flex-shrink: 0;
  border-radius: 12px;
  background: var(--bg-white, #fff);
  margin-bottom: 2%;
}

.pers-details-title h3 {
  color: #1c1c1c;
  font-size: 20px;
  font-style: normal;
  font-weight: 600;
  line-height: normal;
  margin-bottom: -18px;
  top: 177px;
}

.form-group label {
  color: var(--Text-light, #5a5a5a);
  font-size: 14px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%;
}

.form-group {
  display: flex;
  flex-direction: column;
  flex-wrap: wrap;
  justify-content: space-evenly;
  align-items: flex-start;
  position: relative;
  gap: 4px;
}

.tit-and-fullName,
.gen-and-dob,
.med-and-spec {
  display: flex;
  gap: 30px;
  margin-top: 32px;
  flex-wrap: wrap;
}

.form-group > select {
  display: flex;
  padding: 10px;
  align-items: center;
  gap: 8px;
  flex: 1 0 0;
  align-self: stretch;
  width: 300px;
  height: 48px;
  color: var(--Colors-Text-Colors-Text-Primary, #1c1c1c);
  font-size: 14px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%;
  border: 1px solid #eee;
  border-radius: 4px;
  background: var(--bg-white, #fff);
  appearance: none;
  background-image: url("../assets/alt-arrow-down.png");
  background-repeat: no-repeat;
  background-position: right 10px center;
  background-size: 20px;
}
.form-group > input {
  display: flex;
  padding: 10px;
  align-items: center;
  gap: 8px;
  flex: 1 0 0;
  align-self: stretch;
  width: 300px;
  height: 48px;
  color: var(--Colors-Text-Colors-Text-Primary, #1c1c1c);
  font-size: 14px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%;
  border: 1px solid #eee;
  box-sizing: border-box;
  border-radius: 4px;
  background: var(--bg-white, #fff);
}

.form-group select,
input {
  outline: none;
}

#title option,
select {
  color: #1c1c1c;
  font-size: 16px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%;
}

.dob-picker::-webkit-calendar-picker-icon {
  display: none;
}

.dob-picker::-webkit-clear-button {
  display: none;
}

.tit-and-fullName .form-group {
  content: "";
}

#title option,
select:focus {
  outline: none;
}
#gender option,
 select {
  color: #1c1c1c;
  font-size: 16px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%;
  outline: none;

}

#medCred option,
select {
   color: #1c1c1c;
   font-size: 16px;
   font-style: normal;
   font-weight: 400;
   line-height: 150%;
   outline: none;

}

.button-group {
  margin-top: 30px;
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  position: relative;
}

.cancel-btn {
  display: flex;
  height: 42px;
  padding: 8px 20px;
  margin-right: 25px;
  justify-content: center;
  align-items: center;
  gap: 8px;
  flex-shrink: 0;
  align-self: stretch;
  border-radius: 8px;
  border: 1px solid #eee;
  background: var(--bg-white, #fff);
  cursor: pointer;
}

.delete-btn{
  display: flex;
  height: 42px;
  justify-content: center;
  align-items: center;background-color: red;
  border: 1px solid red;
  color: #fff;
  padding: 8px 20px;
  border-radius: 8px;
  cursor: pointer;
  gap: 8px;
  flex-shrink: 0;
  align-self: stretch;
}

.save-btn {
  display: flex;
  height: 42px;
  padding: 8px 20px;
  justify-content: center;
  align-items: center;
  gap: 8px;
  flex-shrink: 0;
  align-self: stretch;
  border-radius: 8px;
  background: var(--Primary-Colors, #0066d4);
  cursor: pointer;
  border: none;
  color: var(--Text-Text-white, var(--bg-white, #fff));
  font-style: normal;
  font-weight: 500;
  line-height: 150%;
}


input[type="date"]::placeholder {
  color: transparent;
}

.profile-content {
  position: relative;
  display: flex;
  flex-direction: column;
  height: 139.16px;
  flex-shrink: 0;
  align-items: center;
  width: 212px;
  bottom: 83px;
  overflow-x: clip;
}

.profile-content h3 {
  color: var(--Colors-Text-Colors-Text-Primary, #1c1c1c);
  font-size: 16px;
  font-style: normal;
  font-weight: 600;
  line-height: 120%;
  max-width: 150px;
  white-space: nowrap;
  text-overflow: ellipsis;
  cursor: pointer;
  overflow:inherit;
}

.profile-content p {
  color: var(--Colors-Text-Colors-Text-Secondary, #666);
  font-size: 14px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%;
  margin-top: -12px;
  text-align: center;
  width: 200px;
  max-width: 150px;
  white-space: nowrap;
  text-overflow: ellipsis;
  cursor: pointer;
  overflow: inherit;
}

.profile-image {
  width: 80px;
  height: 80px;
  flex-shrink: 0;
  border-radius: 50%;
  object-fit: cover;
}

.placeholder-image {
  width: 80px;
  height: 80px;
  flex-shrink: 0;
  background-color: #e0e0e0;
  border-radius: 50%;
  background-image: url("../assets/user_icon.jpg");
  background-size: cover;
}

.edit-icon {
  position: absolute;
  right: 66px;
  bottom: 61px;
  background-color: white;
  border-radius: 50%;
  padding: 4px;
  cursor: pointer;
}

.form-group > select > option {
  display: flex;
  width: 300px;
  padding: 10px;
  flex-direction: column;
  align-items: flex-start;
  gap: 8px;
  border-radius: 8px;
  border: 1px solid var(--Colors-Border-Colors-Border-light, #eef2f7);
  background: var(--bg-white, #fff);
  box-shadow: 0px 4px 12px 0px #e3ecfb;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.error {
  color: red;
  font-size: 12px;
  position: absolute;
  top: 75px;
}

input[type="date"]::-webkit-calendar-picker-indicator {
  width: 20px;
  height: 20.25px;
  flex-shrink: 0;
  position: absolute;
  right: 10px;
  background-image: url("../assets/Subtract.png");
}

.custom-select::-ms-expand {
  display: none;
}

.prof-details-title {
  margin-top: 32px;
}
input[type="date"]:required:invalid::-webkit-datetime-edit {
  color: transparent;
}

input[type="date"]:focus::-webkit-datetime-edit {
  color: black !important;
}
.header-section {
  box-sizing: border-box;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: white;
  padding: 40px;
  border-bottom: 1px solid #ddd;
  width: calc(100% - 80px);
  margin-left: 80px;
  position: sticky;
  top: 0;
  z-index: 1;
}
.doctor-icon {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  position: absolute;
  right: 30px;
  top: 22px;
  cursor: pointer;
  object-fit: cover;
}
.container {
  height: 100vh;
  overflow-y: auto;
  position: relative;
}
.error-image-upload{
  color: red;
    font-size: 12px;
    position: absolute;
    top: 42%;
}
.form-container{
  max-width: 100%;
}
.personal-details-container{
  display: flex;
    flex-wrap: wrap;
    flex-direction: column;
}
.try-Width{
  
  width: 25px !important;

}
.second-btn-grp{
  display: flex;
}
@media (max-width: 575px){
  .second-btn-grp{
    margin-top: 20px;
  }
}
@media  screen and (min-width: 760px) and (max-width: 1024px) {
  .profile-form{
    margin-left: 3%;
    width: 90%;
  }
}
@media  screen and (max-width: 760px) {
  .profile-form{
    margin-left: 3%;
    margin-right: 3%;
    width: 80%;
  }
  .form-container{
    width: 85%;
  }
  .form-group {
    width: 100%;
  }
  .form-group > select{
    width: 100%;
  }
  .form-group > input{
    width: 100%;
  }
}
</style>
