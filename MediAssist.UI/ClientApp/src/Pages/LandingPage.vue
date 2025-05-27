<template>
  <div class="landing-page">
    <!-- Updated Header -->
    <header class="header">
      <div class="logo" @click="reloadPage">
        <AppLogo />
      </div>
      <nav class="nav-menu">
        <router-link to="/" class="nav-link">Home</router-link>
        <router-link to="/about" class="nav-link">About</router-link>
        <router-link to="/features" class="nav-link">Features</router-link>
        <router-link to="/chooseyourplan" class="nav-link">Pricing</router-link>
      </nav>

      <div class="buttons">
        <router-link to="/login" class="btn login-btn">Login</router-link>
        <router-link to="/signup" class="btn signup-btn">Signup</router-link>
      </div>
    </header>

    <!-- Central Content -->
    <div class="central-box">
      <h1 class="central-heading">{{ store.MediAssistConfigManager.DomainName }} AI: Redefining Medical Consultations</h1>
      <p class="central-paragraph">
        {{ store.MediAssistConfigManager.DomainName }} AI is a breakthrough in medical technology, enhancing consultations with real-time transcriptions, AI-generated diagnosis support, and seamless prescription management.
      </p>
      <button class="demo-button" @click.prevent="showScheduleDemoPopUp = true">
          Schedule a demo
      </button>
    </div>

    <!-- Video Section -->
    <div class="video-section">
        <div class="video-box">
            <video controls>
                <source src="@/assets/demo-video.mp4" type="video/mp4" />
                <track src="subtitles_en.vtt" kind="subtitles" srclang="en" label="English">
                <track src="descriptions.vtt" kind="descriptions" srclang="en" label="Descriptions">
                Your browser does not support the video tag.
            </video>
        </div>
    </div>

    <!-- Benefits Section -->
    <section class="benefits-section">
      <h2 class="benefits-heading">Why Choose Us?</h2>
      <div class="features-grid">
        <BenifitsCard
          v-for="(feature, index) in features"
          :key="index"
          :icon="feature.icon"
          :title="feature.title"
          :description="feature.description"
        />
      </div>

      <h2 class="offer-heading">What We Offer</h2>
      <div class="benefits-container">
        <OffersCard
          v-for="(benefit, index) in benefits"
          :key="index"
          :icon="benefit.icon"
          :title="benefit.title"
          :description="benefit.description"
        />
      </div>
    </section>
    <footer>
      <div class="logo" @click="reloadPage">
        <AppLogo />
      </div>
      <div class="footer-links">
        <a href="#">Terms And Conditions</a>| <a href="#">Privacy Policy</a>|
        <a href="#">Compliance Links</a>|
        <a href="#">Copyright Info</a>
      </div>
    </footer>

    <DeleteAccountSuccessAlert
        v-if="showDeleteSuccessPopup"
        :isVisible="showDeleteSuccessPopup"
        title="Deletion Successful"
        :message = "deleteAccountSuccessAlertMessage"
        @cancel="successCancel"
        />

    <ScheduleDemoPopUp 
      :is-visible="showScheduleDemoPopUp" 
      modal-type="schedule-demo"
      title="Request your Demo"
      description="Fill out your details to schedule a personalized demo"
      submitButtonText="Request your Demo"
      @close="showScheduleDemoPopUp = false"
    />
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
import BenifitsCard from "@/components/BenefitsCard.vue";
import OffersCard from "@/components/OffersCard.vue";
import AppLogo from "@/components/AppLogo.vue";
import DeleteAccountSuccessAlert from "@/components/alert/DeleteSuccessPopUp.vue";
import ScheduleDemoPopUp from './modals/GenericModal.vue';
import { useMyStore } from '@/store/store.ts';

const showDeleteSuccessPopup = ref(false);
const showScheduleDemoPopUp = ref(false);
const deleteAccountSuccessAlertMessage = ref("");
const store = useMyStore();
const features = ref([]);

const benefits = ref([]);

const IntializePage = () => {
  deleteAccountSuccessAlertMessage.value = `Your account has been successfully deleted. Thank you for using ${store.MediAssistConfigManager.DomainName} AI.`;

  features.value = [
    {
      icon: new URL('@/assets/HomePageImages/transform-patient-care.png', import.meta.url).href,
      title: "Transform Patient Care",
      description: "Craft precise reports and prescriptions, improving the patient experience with clarity and detail.",
    },
    {
      icon: new URL('@/assets/HomePageImages/streamlined-cunsultations.png', import.meta.url).href,
      title: "Streamlined Consultations",
      description: "Accelerate your daily tasks with an intuitive interface, helping you focus more on patient care.",
    },
    {
      icon: new URL('@/assets/HomePageImages/advanced-session-management.png', import.meta.url).href,
      title: "Advanced Session Management",
      description: "Easily control when to start, stop, or resume transcriptions based on your consultation needs.",
    },
    {
      icon: new URL('@/assets/HomePageImages/seamles-integration.png', import.meta.url).href,
      title: "Seamless Integration",
      description: `${store.MediAssistConfigManager.DomainName} AI seamlessly integrates with your existing systems, ensuring smooth data flow and minimal disruption.`,
    },
  ];

benefits.value = [
  {
    icon: new URL('@/assets/HomePageImages/medical-assistance.png', import.meta.url).href,
    title: "AI-Assisted Transcription",
    description: "Record consultations effortlessly and generate reports without missing a detail.",
  },
  {
    icon: new URL('@/assets/HomePageImages/easy-appointment.png', import.meta.url).href,
    title: "Intelligent Session Management",
    description: "Complete control over the transcription process at every stage of your consultation.",
  },
  {
    icon: new URL('@/assets/HomePageImages/health-monitoring.png', import.meta.url).href,
    title: "Real-Time Diagnostic Assistance",
    description: "Get accurate AI-backed suggestions for diagnosis and prescription on the go.",
  },
  {
    icon: new URL('@/assets/HomePageImages/security-data.png', import.meta.url).href,
    title: "Comprehensive Summaries",
    description: "Generate detailed, shareable reports that combine summaries and prescriptions in one document.",
  },
];
};

const reloadPage = () => {
  window.location.reload();
};

watch(
  () => store.MediAssistConfigManager.DomainName,
  (newDomainName) => {
    if (newDomainName) {
      IntializePage();
    }
  }
);

onMounted(() => {
  IntializePage();
  if (sessionStorage.getItem('showDeleteSuccessPopup') === 'true') {
    showDeleteSuccessPopup.value = true;
  }
});
const successCancel = async () => {
    showDeleteSuccessPopup.value = false;
}
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Karma:wght@400;500&display=swap");

.landing-page {
  position: relative;
  min-height: 100vh;
  color: #00152d;
  background: linear-gradient(0deg, #fff 25.29%, #edf6ff 74.71%);
  height: 100vh;
  overflow-y: auto;
}

/* Header Section */
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 40px 63px;
}

.logo {
  display: flex;
  align-items: center;
  cursor: pointer;
}

.logo h3 {
  font-size: 24px;
  font-weight: 500;
  color: #0066d4;
  margin: 0;
}

.icon {
  width: 35px;
  height: 40px;
  margin-right: 8px;
}

.nav-menu {
  display: flex;
  gap: 64px;
  flex-grow: 1;
  justify-content: center;
  color: #00152d;
  font-size: 16px;
  font-style: normal;
  font-weight: 500;
  line-height: 150%;
  margin-left: 247px;
}

.nav-link {
  color: #1c1c1c;
  font-size: 16px;
  font-weight: 500;
  text-decoration: none;
  transition: color 0.3s ease;
}

.nav-link:hover {
  color: #0066d4;
}

.buttons {
  display: flex;
  gap: 23px;
}

.btn {
  padding: 8px 20px;
  text-align: center;
  transition: background-color 0.3s, border-color 0.3s;
  border-radius: 8px;
  font-weight: 500;
  text-decoration: none;
  color: var(--Text-Text-white, var(--bg-white, #fff));
  font-size: 18px;
  font-style: normal;
  line-height: 150%;
  width: 153px;
}

.demo-button{
  width: 203px;
  height: 51px;
  top: 399px;
  left: 618px;
  padding: 8px 20px 8px 20px;
  gap: 8px;
  border-radius: 8px;
  border: none;
  background: #0066D4;
  font-size: 18px;
  font-weight: 500;
  line-height: 27px;
  text-align: center;
  text-underline-position: from-font;
  text-decoration-skip-ink: none;
  color: #FFFFFF;
}

.login-btn {
  background-color: #0066d4;
  border: none;
}

.login-btn:hover {
  background-color: #005bb5;
}

.signup-btn {
  background-color: transparent;
  color: #0066d4;
  border: 1px solid #0066d4;
}

.signup-btn:hover {
  background-color: #0066d4;
  color: #fff;
}

/* Central Section */
.central-box {
  margin: 40px auto;
  width: 955px;
  height: 225px;
  text-align: center;
}

.central-heading {
  font-size: 43px;
  font-weight: 600;
  margin-bottom: 20px;
}

.central-paragraph {
  font-weight: 400;
  font-size: 18px;
  line-height: 27px;
  width: 915px;
  height: 81px;
  margin-bottom: 0;
}

/* Video Section */
.video-section {
  display: flex;
  justify-content: center;
  margin: 4px;
}

.video-box {
  width: 1039px;
  height: 494px;
  border-radius: 15px;
  overflow: hidden;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
}

video {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

/* Benefits Section */
.benefits-section {
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.benefits-heading {
  margin-bottom: 58px;
  color: #1c1c1c;
  font-size: 40px;
  font-style: normal;
  font-weight: 600;
  line-height: 150%; /* 60px */
}
.offer-heading {
  color: #1c1c1c;
  margin-bottom: 58px;
  font-size: 40px;
  font-style: normal;
  font-weight: 600;
  line-height: 150%; /* 60px */
}

.benefits-container {
  display: flex;
  justify-content: space-around;
  width: 100%;
  max-width: 1200px;
  gap: 20px;
}

.benefit-box {
  text-align: center;
  width: 150px;
  margin: 10px;
}

.icon-wrapper {
  width: 120px;
  height: 120px;
  margin: 0 auto 0px;
  border-radius: 50%;
  display: flex;
  justify-content: center;
  align-items: center;
}

.benefit-icon {
  width: 80px;
  height: 80px;
}

.icon-wrapper:hover {
  transform: translateY(-5px);
}

.benefit-title {
  font-size: 16px;
  margin: 0px;
  color: #223a66;
}

.benefit-description {
  font-size: 14px;
  color: #353535;
  margin-bottom: 10px;
}

.features-grid {
  display: flex;
  gap: 20px;
  margin-bottom: 40px;
  flex-wrap: wrap;
  margin-inline: 144px;
}

@media (max-width: 768px) {
  .features-grid {
    flex-direction: column;
    align-items: center;
  }
}

@media (max-width: 912px) {
  .central-box {
    width: 90%;
  }

  .central-paragraph {
    width: 90%;
  }

  .video-box {
    width: 100%;
  }

  .features-grid {
    flex-direction: column;
    width: 100%;
    justify-content: center;
  }

  .benefits-container {
    flex-direction: column;
    gap: 20px;
  }
}
@media (min-width: 913px) {
  .features-grid {
    display: grid;
    grid-template-columns: auto auto;
  }
}
footer {
  background-color: #0a192f;
  color: white;
  padding: 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 79px;
  height: 120px;
}

.logo-icon {
  color: #3b82f6;
  font-size: 24px;
  margin-right: 10px;
}
.logo-text {
  font-size: 20px;
  font-weight: bold;
  text-transform: uppercase;
}
.footer-links {
  display: flex;
  gap: 5px;
  margin-right: 60px;
  color: var(--Text-White, var(--bg-white, #fff));
  font-size: 16px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%;
  text-transform: capitalize;
}
.footer-links a {
  color: white;
  text-decoration: none;
}
</style>
