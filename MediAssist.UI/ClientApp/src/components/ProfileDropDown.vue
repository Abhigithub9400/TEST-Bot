<template>
  <div class="profile-dropdown" :class="{show: props.menuOpen}" v-if="props.menuOpen && ismenuopen ">
    <div class="profile-header">
        <img class="profile-image" :src="profileImage" alt="Profile" />
      <div class="profile-info">
          <h3 class="name">{{ title }} {{ firstName }}</h3>
          <p class="role">{{ specialization }}</p>
      </div>
    </div>
    <div class="profile-actions">
      <div class="my-profile">
        <router-link to="/profile-management" class="menu-item">
          <i class="icon">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="16"
              height="17"
              viewBox="0 0 16 17"
              fill="none"
            >
              <path
                fill-rule="evenodd"
                clip-rule="evenodd"
                d="M4.83326 4.21973C4.83326 2.47083 6.25102 1.05306 7.99993 1.05306C9.74883 1.05306 11.1666 2.47083 11.1666 4.21973C11.1666 5.96863 9.74883 7.3864 7.99993 7.3864C6.25102 7.3864 4.83326 5.96863 4.83326 4.21973ZM7.99993 2.05306C6.80331 2.05306 5.83326 3.02311 5.83326 4.21973C5.83326 5.41635 6.80331 6.3864 7.99993 6.3864C9.19654 6.3864 10.1666 5.41635 10.1666 4.21973C10.1666 3.02311 9.19654 2.05306 7.99993 2.05306Z"
                fill="#707070"
              />
              <path
                d="M11.9999 9.4975C12.2761 9.4975 12.4999 9.72136 12.4999 9.9975V10.3864H12.8887C13.1648 10.3864 13.3887 10.6103 13.3887 10.8864C13.3887 11.1625 13.1648 11.3864 12.8887 11.3864H12.4999V11.7753C12.4999 12.0514 12.2761 12.2753 11.9999 12.2753C11.7238 12.2753 11.4999 12.0514 11.4999 11.7753V11.3864H11.1109C10.8348 11.3864 10.6109 11.1625 10.6109 10.8864C10.6109 10.6103 10.8348 10.3864 11.1109 10.3864H11.4999V9.9975C11.4999 9.72136 11.7238 9.4975 11.9999 9.4975Z"
                fill="#707070"
              />
              <path
                fill-rule="evenodd"
                clip-rule="evenodd"
                d="M9.84969 8.56167C9.26788 8.44776 8.64556 8.3864 7.99993 8.3864C6.45756 8.3864 5.03654 8.73698 3.98356 9.32929C2.94625 9.91278 2.16659 10.7971 2.16659 11.8864L2.16655 11.9544C2.1658 12.7289 2.16485 13.701 3.01753 14.3954C3.43718 14.7372 4.02424 14.9802 4.8174 15.1407C5.61277 15.3017 6.64941 15.3864 7.99993 15.3864C9.91199 15.3864 11.2065 15.2171 12.0797 14.8921C12.8656 14.5995 13.3314 14.1708 13.5802 13.6312C14.5284 13.0841 15.1666 12.0598 15.1666 10.8864C15.1666 9.1375 13.7488 7.71973 11.9999 7.71973C11.1699 7.71973 10.4144 8.0391 9.84969 8.56167ZM4.47382 10.2009C3.58085 10.7032 3.16659 11.3188 3.16659 11.8864C3.16659 12.7583 3.19347 13.2491 3.64899 13.62C3.89601 13.8212 4.30894 14.0175 5.01579 14.1606C5.72042 14.3032 6.68378 14.3864 7.99993 14.3864C9.71858 14.3864 10.8266 14.2441 11.5405 14.02C10.0093 13.7974 8.83326 12.4793 8.83326 10.8864C8.83326 10.3745 8.95472 9.89097 9.1704 9.46306C8.7959 9.41304 8.40396 9.3864 7.99993 9.3864C6.59678 9.3864 5.35112 9.70738 4.47382 10.2009ZM9.83326 10.8864C9.83326 9.68978 10.8033 8.71973 11.9999 8.71973C13.1965 8.71973 14.1666 9.68978 14.1666 10.8864C14.1666 12.083 13.1965 13.0531 11.9999 13.0531C10.8033 13.0531 9.83326 12.083 9.83326 10.8864Z"
                fill="#707070"
              />
            </svg>
          </i>
          <div class="my-profile-name">My Profile</div>
          <div class="arrow">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="16"
              height="17"
              viewBox="0 0 16 17"
              fill="none"
            >
              <path
                fill-rule="evenodd"
                clip-rule="evenodd"
                d="M5.67461 3.67343C5.88428 3.49372 6.19993 3.518 6.37964 3.72766L10.3796 8.39433C10.5401 8.58157 10.5401 8.85787 10.3796 9.04512L6.37964 13.7118C6.19993 13.9214 5.88428 13.9457 5.67461 13.766C5.46495 13.5863 5.44067 13.2707 5.62038 13.061L9.34147 8.71972L5.62038 4.37845C5.44067 4.16879 5.46495 3.85314 5.67461 3.67343Z"
                fill="#707070"
              />
            </svg>
          </div>
        </router-link>
      </div>  
      <hr>
      <div class="change-password-container">
        <a href="#" @click.prevent="openChangePasswordPopup" class="menu-item">
          <i class="icon"
            ><svg
              xmlns="http://www.w3.org/2000/svg"
              width="16"
              height="17"
              viewBox="0 0 16 17"
              fill="none"
            >
              <path
                d="M5.99992 10.8864C5.99992 11.2546 5.70144 11.5531 5.33325 11.5531C4.96506 11.5531 4.66659 11.2546 4.66659 10.8864C4.66659 10.5182 4.96506 10.2197 5.33325 10.2197C5.70144 10.2197 5.99992 10.5182 5.99992 10.8864Z"
                fill="#707070"
              />
              <path
                d="M8.66659 10.8864C8.66659 11.2546 8.36811 11.5531 7.99992 11.5531C7.63173 11.5531 7.33325 11.2546 7.33325 10.8864C7.33325 10.5182 7.63173 10.2197 7.99992 10.2197C8.36811 10.2197 8.66659 10.5182 8.66659 10.8864Z"
                fill="#707070"
              />
              <path
                d="M10.6666 11.5531C11.0348 11.5531 11.3333 11.2546 11.3333 10.8864C11.3333 10.5182 11.0348 10.2197 10.6666 10.2197C10.2984 10.2197 9.99992 10.5182 9.99992 10.8864C9.99992 11.2546 10.2984 11.5531 10.6666 11.5531Z"
                fill="#707070"
              />
              <path
                fill-rule="evenodd"
                clip-rule="evenodd"
                d="M3.49992 5.55306V6.42157C3.3487 6.43221 3.20617 6.44605 3.07209 6.46408C2.47201 6.54476 1.96676 6.71735 1.56549 7.11863C1.16421 7.51991 0.991612 8.02516 0.910935 8.62523C0.833228 9.20321 0.833239 9.93808 0.833252 10.8498V10.923C0.833239 11.8347 0.833228 12.5696 0.910935 13.1476C0.991612 13.7476 1.16421 14.2529 1.56549 14.6542C1.96676 15.0554 2.47201 15.228 3.07209 15.3087C3.65006 15.3864 4.38495 15.3864 5.29667 15.3864H10.7032C11.6149 15.3864 12.3498 15.3864 12.9278 15.3087C13.5278 15.228 14.0331 15.0554 14.4344 14.6542C14.8356 14.2529 15.0082 13.7476 15.0889 13.1476C15.1666 12.5696 15.1666 11.8347 15.1666 10.923V10.8498C15.1666 9.93809 15.1666 9.20321 15.0889 8.62523C15.0082 8.02516 14.8356 7.51991 14.4344 7.11863C14.0331 6.71735 13.5278 6.54476 12.9278 6.46408C12.7937 6.44605 12.6511 6.43221 12.4999 6.42157V5.55306C12.4999 3.06778 10.4852 1.05306 7.99992 1.05306C5.51464 1.05306 3.49992 3.06778 3.49992 5.55306ZM7.99992 2.05306C6.06692 2.05306 4.49992 3.62007 4.49992 5.55306V6.38869C4.74891 6.38639 5.01427 6.38639 5.29658 6.3864H10.7032C10.9855 6.38639 11.2509 6.38639 11.4999 6.38869V5.55306C11.4999 3.62007 9.93292 2.05306 7.99992 2.05306ZM3.20533 7.45516C2.71615 7.52093 2.4571 7.64123 2.27259 7.82574C2.08808 8.01024 1.96779 8.26929 1.90202 8.75848C1.83431 9.26204 1.83325 9.92945 1.83325 10.8864C1.83325 11.8433 1.83431 12.5107 1.90202 13.0143C1.96779 13.5035 2.08808 13.7625 2.27259 13.9471C2.4571 14.1316 2.71615 14.2519 3.20533 14.3176C3.7089 14.3853 4.37631 14.3864 5.33325 14.3864H10.6666C11.6235 14.3864 12.2909 14.3853 12.7945 14.3176C13.2837 14.2519 13.5427 14.1316 13.7272 13.9471C13.9118 13.7625 14.0321 13.5035 14.0978 13.0143C14.1655 12.5107 14.1666 11.8433 14.1666 10.8864C14.1666 9.92945 14.1655 9.26204 14.0978 8.75848C14.0321 8.26929 13.9118 8.01024 13.7272 7.82574C13.5427 7.64123 13.2837 7.52093 12.7945 7.45516C12.2909 7.38746 11.6235 7.3864 10.6666 7.3864H5.33325C4.37631 7.3864 3.7089 7.38746 3.20533 7.45516Z"
                fill="#707070"
              />
            </svg>
          </i>
          <div class="change-password">Change Password</div>
          <div class="arrow">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="16"
              height="17"
              viewBox="0 0 16 17"
              fill="none"
            >
              <path
                fill-rule="evenodd"
                clip-rule="evenodd"
                d="M5.67461 3.67343C5.88428 3.49372 6.19993 3.518 6.37964 3.72766L10.3796 8.39433C10.5401 8.58157 10.5401 8.85787 10.3796 9.04512L6.37964 13.7118C6.19993 13.9214 5.88428 13.9457 5.67461 13.766C5.46495 13.5863 5.44067 13.2707 5.62038 13.061L9.34147 8.71972L5.62038 4.37845C5.44067 4.16879 5.46495 3.85314 5.67461 3.67343Z"
                fill="#707070"
              />
            </svg>
          </div>
        </a>
      </div>
      <button @click="showLogoutConfirmation" class="logout-btn">Logout</button>
    </div>
  </div>
  <div> 
    <ConfirmationAlert
      v-if="isLogoutModalVisible"
      :isVisible="isLogoutModalVisible"
      title="Confirm Logout"
      message="Are you sure you want to log out of the application?"
      @confirm="handleLogout"
      @cancel="close"
    /></div>
  <div>
    <!-- Change Password Popup -->
    <ChangePassword v-if="isChangePasswordModalVisible" @close="close" />
  </div>  
</template>

<script setup>
import {defineProps, ref,onMounted, onBeforeUnmount} from 'vue'
import ConfirmationAlert from './alert/LogoutAlert.vue';
import ChangePassword from './ChangePassword.vue';
import { useRouter } from 'vue-router';
import Icon from "../assets/user_icon.jpg";
import axiosInstance from '../Services/Interceptors/axios.js';
const router = useRouter();
const props = defineProps({
  menuOpen : Boolean
})
const isLogoutModalVisible = ref(false);
const isChangePasswordModalVisible = ref(false); 
const ismenuopen = ref(true);
const firstName = ref("");
const title = ref("");
const specialization = ref("");
const userId = ref("");
const profileImage = ref("");
const showLogoutConfirmation = () => {
  isLogoutModalVisible.value = true;
  ismenuopen.value = false;
};
const openChangePasswordPopup = () => {
  isChangePasswordModalVisible.value = true; 
  ismenuopen.value = false;
};

function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return decodeURIComponent(parts.pop().split(';').shift());
  return null;
}
const handleLogout = async () =>  {
  try{
    await axiosInstance.post('/api/manage/logout');
    localStorage.clear();
    sessionStorage.clear();
     const clearCookie = (name) => {
      document.cookie = `${name}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT; SameSite=Strict`;
    };
     ['userId', 'firstName', 'specialization', 'title'].forEach(clearCookie);
      localStorage.setItem('userLoggedOut', Date.now());
      redirectAndLockToLoginPage();

    isLogoutModalVisible.value = false;
    ismenuopen.value = true;
  }catch (error) {
    console.error("Logout failed", error);
  }
  
};

function redirectAndLockToLoginPage() {
  router.replace('/login');

  setTimeout(() => {
    history.replaceState(null, null, '/login'); 
    history.pushState(null, null, '/login');

    window.onpopstate = function () {
      history.pushState(null, null, '/login');
    };
  }, 0); 
}
window.addEventListener('storage', (event) => {
    if (event.key === 'userLoggedOut') {
      redirectAndLockToLoginPage();
    }
});

const close = () => {
  ismenuopen.value = true;
  isLogoutModalVisible.value = false;
  isChangePasswordModalVisible.value = false;
}

onMounted(() => {
  userId.value = getCookie("userId");
  firstName.value = getCookie("firstName");
  const titleValue = getCookie("title");
  title.value = (titleValue === null || titleValue === "null") ? '' : titleValue;
  const specializationValue = getCookie("specialization");
  specialization.value = (specializationValue === null || specializationValue === "null") ? '' : specializationValue;
  const imageValue = localStorage.getItem("image");

  let imageType = "png";
    if (imageValue && imageValue !== "null") {
      if (imageValue.startsWith("/9j")) {
        imageType = "jpeg";
      }
      profileImage.value = `data:image/${imageType};base64,${imageValue}`;
    }
    else{
      profileImage.value = Icon;
    }
  window.addEventListener("profile-updated", updateProfile);
  window.addEventListener("settings-updated", (event) => {
    if (event.detail.success) {
      specialization.value = event.detail.specialization;
    }
  });
});

onBeforeUnmount(() => {
  // Clean up the event listener when the component is destroyed
  window.removeEventListener("profile-updated", updateProfile);
});

function updateProfile() {

  const titleValue = getCookie("title");
  title.value = (titleValue === null || titleValue === "null") ? '' : titleValue;

  const specializationValue = getCookie("specialization");
  specialization.value = (specializationValue === null || specializationValue === "null") ? '' : specializationValue;

   const imageValue = localStorage.getItem("image");
   if (imageValue && imageValue !== "null") {
    let imageType = "png";
    if (imageValue.startsWith("/9j")) {
      imageType = "jpeg";
    }
    profileImage.value = `data:image/${imageType};base64,${imageValue}`;
  } else {
    profileImage.value = Icon;
  }

}
</script>

<style scoped>
hr {
  width: 227px;
  height: 1px;
  background: #E3E9F1;
  border: none;
}
.change-password{
  margin-right: 64px;
}
.my-profile-name{
  margin-right: 110px;
}
.arrow {
  width: 16px;
  height: 16px;
  margin-right: 10px;
  fill: none;
}
.icon {
  width: 16px;
  height: 16px;
  margin-right: 10px;
}
.menu-item {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  color: var(--Colors-Text-Colors-Text-Primary, #1c1c1c);
  text-align: center;
  font-size: 14px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%;
  text-decoration: none;
  width: 240px;
}
.my-profile,
.change-password-container {
  display: flex;
  flex-direction: row;
}
.profile-dropdown {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  position: absolute;
  height: 250px;
  right: 20px;
  top: 70px;
  z-index: 37;
}
.profile-dropdown.show{
  display: block;
}
.profile-header {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-bottom: 20px;
  width: 258.923px;
  height: 140.72px;
  flex-shrink: 0;
  border-radius: 12px 12px 0px 0px;
  background: #003482;
  gap: 8px;
}

.profile-image {
  width: 48px;
  height: 48px;
  border-radius: 41px;
  border: 2px solid #fff;
  margin-right: 10px;
  flex-shrink: 0;
  stroke-width: 1.5px;
  stroke: #e6f0ff;
  margin-top: 18px;
  margin-bottom: -10px;
  object-fit: cover;
}

.profile-info .name {
  width: 238.691px;
  color: var(--bg-white, #fff);
  text-align: center;
  font-size: 16px;
  font-style: normal;
  font-weight: 600;
  line-height: 120%;
  margin-top: 8px;
}

.profile-info .role {
  color: var(--bg-white, #fff);
  text-align: center;
  gap: 4px;
  font-size: 12px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%; /* 18px */
   margin-top: -11px;
}

.profile-actions {
  position: absolute;
  top: 132px;
  display: flex;
  width: 228px;
  padding: 20px 16px;
  flex-direction: column;
  align-items: flex-start;
  gap: 5px;
  border-radius: 12px;
  background: var(--bg-white, #fff);
  box-shadow: 0px 2px 10px 0px #E3ECFB;
}

.profile-action {
  display: flex;
  width: 227px;
  padding: 20px 16px;
  flex-direction: column;
  align-items: flex-start;
  gap: 10px;
  border-radius: 12px;
  background: var(--bg-white, #fff);
}
.profile-action a {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  align-self: stretch;
  width: 100px;
}
.profile-action .icon {
  margin-right: 8px;
}

.user-icon {
  background: url("@/assets/doctor-icon.png") lightgray -26.2px -0.675px /
    229.838% 153.302% no-repeat;
  width: 16px;
  height: 16px;
}

.lock-icon {
  background: url("@/assets/doctor-icon.png") no-repeat center center;
  width: 16px;
  height: 16px;
}

.logout-btn {
  border: none;
  color: white;
  font-size: 14px;
  cursor: pointer;
  font-weight: bold;
  background: #0066d4;
  width: 227px;
  height: 37px;
  border-radius: 4px;
  display: flex;
  padding: 8.5px 10px;
  justify-content: center;
  align-items: center;
  gap: 6px;
  flex: 1 0 0;
  align-self: stretch;
  margin-top: 20px;
}

.logout-btn:hover {
  background-color: #0056b3;
}
</style>
