<template>
  <div class="home-page">
    <div class="background-section">
      <header class="header-container">
        <hamburger-menu></hamburger-menu>
        <div class="user-menu">
          <div class="toast" v-if="showToast">
            <div
              class="profile-update"
              :class="{ success: success }"
            >
              <img
                v-if="success"
                class="bold-essentional-ui-check"
                alt="success icon"
                src="../assets/settings_update_icon.png"
              />
              <div class="profile-updated-successfully">
                {{ message }}
              </div>
              <router-link to="/settings">
                <button class="updateSettings">
                  Update Settings
                </button>
              </router-link>
              <button class="image-delete-icon" @click="showToast = false">
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
          <div class="app-logo">
            <AppLogo :fromDashboard= "fromDashboard =true"  />
          </div>
          <div class="settings">
             <router-link to="/settings">
             <img src="@/assets/settings_icon.png" alt="settings_icon">
             </router-link>
            </div>
          <div>
            <img
              :src="image"
              alt="User Icon"
              class="user-icon"
              @click="toggleMenu"
            />
          </div>
          <div class="user-profile">
            <ProfileDropDown :menuOpen="menuOpen" />
          </div>
        </div>
        <div class="content-area">
            <h1 class="welcome-heading">
                Hi {{ firstName }}, Welcome to {{store.MediAssistConfigManager.DomainName}}
            </h1>
          <p class="welcome-text">Our AI-driven medical scribing application provides real-time insights, predictive analytics, and detailed reporting to ensure superior patient care and more efficient medical processes.
          </p>
          <div class="button-group">
            <router-link to="/conversation" class="start-chat-button"
              >Launch Consultation</router-link
            >
          </div>
        </div>
        <div class="feedback-form" @click="showFeedbackForm">
          <div class="feedback-button">Submit Feedback</div>
        </div>
      </header>
    </div>
    <div class="key-features-container">Key Features</div>

    <div class="key-features">
      <div class="features-grid">
        <FeatureCard
          v-for="(feature, index) in benefits"
          :key="index"
          :icon="feature.icon"
          :title="feature.title"
          :description="feature.description"
        />
      </div>
    </div>

    <FeedBackForm
      :isVisible="showpopup"
      :title="FeedbackTitle"
      :message="feedBack"
      :cancelText="cancelText"
      :confirmText="confirmText"
      @confirm="confirmationRequired"
      @cancel="cancelAction"
    />

    <FeedbackPopup
      :isVisible="showFeedbackPopup"
      :title="PopupTitle"
      :confirmText="FeedbackMessage"
      @confirm="confirmPopup"
      @cancel="cancelPopup"
      @setRating="setRating"
      @update:selectedItems="handleSelectedCategories"
      @text:otherCategoryText="handleCustomCategoryText"
    />

    <FeedbackSuccessPopup
      :isVisible="showSucessPopup"
      :title="sucessTitle"
      :message="sucessMessage"
      @cancel="successCancel"
    />

    <FeedbackErrorPopup
      :isVisible="showErrorPopup"
      :isError="showErrorPopup"
      :title="erorTitle"
      :message="errorMessage"
      :tryAgain="tryAgainButton"
      @confirm="tryAgain"
      @cancel="successCancel"
    />
  </div>
</template>

<script setup>


import Icon from "@/assets/doctor-icon.png";
import FeedBackForm from "@/components/alert/ConfirmationAlert.vue";
import FeedbackPopup from "@/components/alert/FeedbackPopup.vue";
import { default as FeedbackErrorPopup, default as FeedbackSuccessPopup } from "@/components/alert/FeedbackSuccessErrorPopup.vue";
import AppLogo from "@/components/AppLogo.vue";
import FeatureCard from "@/components/FeatureCard.vue";
import ProfileDropDown from "@/components/ProfileDropDown.vue";
import axiosInstance from "@/Services/Interceptors/axios";
import { useMyStore } from '@/store/store.ts';
import { onBeforeMount, onMounted, onUnmounted, ref } from "vue";

const fromDashboard = ref(false);
const menuOpen = ref(false);
const firstName = ref("");
const FeedbackTitle = ref("We Value Your Feedback!");
const FeedbackMessage = ref("Submit Feedback");
const cancelText = ref("Maybe Later");
const confirmText = ref("Submit");
const showpopup = ref(false);
const showFeedbackPopup = ref(false);
const feedBack = ref(
  "Please provide your valuable insights to help us improve your experience."
);
const PopupTitle = ref("Share Your Feedback");
const sucessTitle = ref("Submission Successful");
const sucessMessage = ref(
  "Thank you for your feedback! It has been successfully submitted."
);
const showSucessPopup = ref(false);
const showErrorPopup = ref(false);
const erorTitle = ref("Submission Failed");
const errorMessage = ref(
  "There was a problem submitting your feedback. Please try again later."
);
const tryAgainButton = ref("Try Again");
const rating = ref(0);
const image = ref("");
const userId = ref("");
const selectedCategories = ref([]);
const customCategoryText = ref("");
const store = useMyStore();
const showToast = ref(false);
const message = ref("");
const success = ref(false);

const handleSelectedCategories = (categories) => {
  selectedCategories.value = categories.map(category => category.id);
};

const handleCustomCategoryText = (text) => {
  customCategoryText.value = text;
}

const toggleMenu = () => {
  menuOpen.value = !menuOpen.value;
};

const confirmPopup = async (feedbackData) => {
  try {

    const descriptiontest= feedbackData.description;
    const suggestiontest = feedbackData.suggestion;

    const payload = {
    userId: userId.value,
    CategoryIDs: selectedCategories.value,
    Rating: rating.value,
    CustomCategoryText :customCategoryText.value,
    IssueDescription: descriptiontest,
    SuggestionsImprovement: suggestiontest
  };

  const response = await axiosInstance.post("/api/feedback/addfeedback", payload);

    if (response.status === 200 && response.data.success) {
      showSucessPopup.value = true;
      showFeedbackPopup.value = false;
    } else {
      showErrorPopup.value = true;
    }
  } catch (error) {
    console.error("Error during feedback submission:");
  }
};


const cancelPopup = () => {
  showFeedbackPopup.value = false;
};
const confirmationRequired = () => {
  showFeedbackPopup.value = true;
  showpopup.value = false;
};
const showFeedbackForm = () => {
  showpopup.value = true;
};

const cancelAction = () => {
  showpopup.value = false;
};

const successCancel = () => {
  showSucessPopup.value = false;
  showErrorPopup.value = false;
};

const tryAgain = () => {
  showSucessPopup.value = false;
  showErrorPopup.value = false;
};

const setRating = (star) => {
  rating.value = star;
};

const handleClickOutside = (event) => {
  const menu = document.querySelector(".user-profile");
  const userIcon = document.querySelector(".user-icon");
  if (
    menu &&
    !menu.contains(event.target) &&
    !userIcon.contains(event.target)
  ) {
    menuOpen.value = false;
  }
};

let benefits = [
  {
    icon: require("@/assets/dashBoardImages/effortless_real-time _transcription.jpg"),
    title: "Effortless Real-Time Transcription",
    description:
      "Capture and convert conversations into structured reports in seconds.",
  },
  {
    icon: require("@/assets/dashBoardImages/smart_session_controls.jpg"),
    title: "Smart Session Controls",
    description:
      "Pause, resume, or end transcriptions with total ease designed for your workflow.",
  },
  {
    icon: require("@/assets/dashBoardImages/ai_powered_diagnostics.jpg"),
    title: "AI-Powered Diagnostics",
    description:
      "Leverage AI to receive instant diagnosis suggestions and automatic prescription drafts.",
  },
  {
    icon: require("@/assets/dashBoardImages/comprehensive_reporting.jpg"),
    title: "Comprehensive Reporting",
    description:
      "Generate precise, comprehensive summaries, ready for sharing or archiving.",
  },
];

function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
  return null;
}

onBeforeMount(async () => { 
  let userId = getCookie("userId");
  await store.fetchUserActivityMetrics(userId);
 })

onMounted(() => {
  userId.value = getCookie("userId");
  firstName.value = getCookie("firstName");
  const imageValue = localStorage.getItem("image");

  let imageType = "png";
  if (imageValue && imageValue !== "null") {
    if (imageValue.startsWith("/9j")) {
      imageType = "jpeg";
    }
    image.value = `data:image/${imageType};base64,${imageValue}`;
  } else {
    image.value = Icon;
  }

  window.addEventListener("update-settings", handleUpdateSettings);

  document.addEventListener("click", handleClickOutside);  
});

onUnmounted(() => {
  window.removeEventListener("update-settings", handleUpdateSettings);
  document.removeEventListener("click", handleClickOutside);
});

function handleUpdateSettings(event) {
  const { success: isSuccess, message: eventMessage } = event.detail;
  success.value = isSuccess;
  message.value = eventMessage;
  showToast.value = true;
  setTimeout(() => {
    showToast.value = false;
  }, 8000);
}

</script>

<style scoped>
.home-page {
  color: #333;
  height: 100vh;
  overflow-y: auto;
  position: relative;
}

.header-container {
  box-sizing: border-box;
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;

  padding: 20px;
  z-index: 3;
}
.header-container .user-menu {
  position: absolute;
  width: 98%;
  display: flex;
  top: -1px;
}

.app-logo {
  width: 100%;
  display: flex;
  align-items: center;
  margin-left: 47px;
}

.user-icon {
  padding: 19px 28px;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  -o-object-fit: cover;
  object-fit: cover;
  cursor: pointer;
}

.settings{
  position: absolute;
  right: 90px;
  top: 27px;
}
.dropdown-menu {
  position: absolute;
  top: 100%;
  right: 15px;
  background-color: #fff;
  border: 1px solid #ddd;
  border-radius: 4px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
  width: 150px;
  z-index: 10;
}

.dropdown-menu .menu-item {
  padding: 10px;
  text-align: left;
  text-decoration: none;
  color: #333;
  font-size: 14px;
  font-weight: normal;
}

.dropdown-menu .menu-item.logout {
  border: none;
  background-color: #fff;
}

.dropdown-menu .menu-item:hover {
  background-color: #f0f0f0;
}

.background-section {
  position: relative;
  width: 100%;
  height: 400px;
  display: flex;
  align-items: center;
}

.background-section::before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-image: url("@/assets/dashBoardImages/dashboard-background.jpg");
  background-size: cover;
  background-position: center;
  z-index: 1;
}

.background-section::after {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: linear-gradient(
    221.47deg,
    rgba(0, 13, 32, 0) -126.54%,
    rgba(0, 9, 22, 0.82) 76.54%
  );
  z-index: 2;
}

.content-area {
  position: relative;
  max-width: 1301px;
  color: white;
  box-sizing: border-box;
  z-index: 3;
  overflow: hidden;
  margin-top: 100px;
  margin-left: 5%;
}

.welcome-heading {
  font-size: 50px;
  margin-bottom: 10px;
  text-transform: capitalize;
}

.welcome-text {
  margin-bottom: 33px;
  overflow: hidden;
  color: var(--bg-white, #fff);
  font-size: 20px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%;
}

.button-group {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

.feedback-form {
  display: flex;
  flex-wrap: wrap;
  align-items: right;
  position: absolute;
  rotate: -90deg;
  top: 194px;
  right: -56px;
}

.feedback-button {
  padding: 10px 20px;
  border: 1px #0066d4;
  border-radius: 5px;
  color: white;
  cursor: pointer;
  font-size: 14px;
  font-weight: 100;
  text-decoration: none;
  background-color: #0066d4;
}

.start-chat-button,
.upload-scan-button {
  padding: 10px 20px;
  border: none;
  border-radius: 5px;
  color: white;
  cursor: pointer;
  font-size: 1em;
  text-decoration: none;
}

.start-chat-button {
  background-color: #0066d4;
}


.feedback-button:hover {
  background-color: #005bb5;
}
.upload-scan-button {
  background-color: #ffffff;
  color: black;
}

.start-chat-button:hover {
  background-color: #005bb5;
}

.upload-scan-button:hover {
  background-color: #dcdfe1;
}

.key-features {
  display: grid;
  padding: 48px 48px 20px 48px;
  box-sizing: border-box;
}

.key-features-container {
  display: flex;
  color: #1c1c1c;
  font-size: 31px;
  font-style: normal;
  font-weight: 600;
  line-height: 150%; /* 46.5px */
  height: 47px;
  width: 251px;
  margin-top: 45px;
  margin-left: 48px;
}

.feature {
  width: 45%;
}

.icon {
  font-size: 2em;
  margin-bottom: 10px;
}

h2 {
  font-size: 1.5em;
  margin-bottom: 10px;
}

p {
  font-size: 1em;
}

.features-grid {
  display: flex;
    
}
    @media (max-width: 1024px) {
     
      .content-area{
        margin-top: 60px;
      }
      .welcome-heading{
        font-size: 40px;
      }
      .welcome-text{
        max-width:95%;
      }
      .features-grid{
        flex-direction: column;
            align-items: center;
            display: ruby;
      }
    }
    @media (max-width: 768px) {
       /* can change the screen size */
        .features-grid {
            flex-direction: column;
            align-items: center;
            display: ruby;
        }
        .background-section{
        height: 75%;
      }
      .welcome-text{
        max-width: 67%;
        font-size: 20px ;  
      }
      .welcome-heading{
        font-size: 35px;
        max-width: 83%;
        margin-top:10px
      }
      .key-features-container{
        margin-top: 20px;
      }     
    }
    @media screen and (max-width: 560px) {
      .background-section{
        height: 75%;
      }
      .welcome-text{
        font-size: 17px;
      }
      .welcome-heading{
        font-size: 33px;
      }
    }
    @media screen and (max-width: 420px) {    
    .key-features{
      padding: 5%;
    }
    .key-features-container{
      margin-bottom:5%;
    }
    .background-section{
        height: 70%;
      }
      .welcome-text{
        font-size: 18px;
        max-width: 90%;
      }
      .welcome-heading{
        font-size: 28px;
      }
    }

@media (max-width: 768px) {
  .features-grid {
    flex-direction: column;
    align-items: center;
  }
}

.settings img {
  filter: brightness(0) invert(1);
  transition: filter 0.3s ease; 
}

.settings img:hover {
  filter: none; 
  background-color: #0066D4; 
  background-blend-mode: color;
  mask-image: url('@/assets/settings_icon.png');
}

.toast {
  position: absolute;
  top: 26px;
  width: 765px;
  height: 54px;
  left: 368px;
  text-align: center;
}

.profile-updated-successfully {
  position: relative;
  line-height: 26px;
  width: 87%;
  font-size: 15px;
  color: #1c1c1c;
  flex-grow: 1;
}

.profile-update.errorUpdate {
  background-color: #fff;
}
.profile-update.success {
  background-color: #fff;
}
.profile-update {
  display: inline-flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-start;
  gap: 7px;
  width: 85%;
  padding: 10px;
  border-radius: 4px;
  position: relative;
}

.bold-essentional-ui-check,
.bold-essentional-ui-error {
  width: 28px;
  height: 28px;
}

.image-delete-icon {
  border: 0;
  background: none;
  cursor: pointer;
  padding-top: 2px;
}

.updateSettings{
  background: #167BE8;
  border: 1px solid #167BE8;
  border-radius: 4px;
  width: 145px;
  height: 34px;
  color: #fff;
  cursor: pointer;
}

</style>
