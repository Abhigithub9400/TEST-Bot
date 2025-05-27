<template>
  <div class="parent-container">
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
    <div class="main-content">
      <div class="frame">
        <div class="frame-wrapper">
          <div class="div-wrapper">
            <div class="div">
              <div class="text-wrapper">Settings</div>
            </div>
          </div>
        </div>
      </div>
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
      <div class="panelframe">
        <div class="sidepanel">
        <div class="hospital" >
          <a
              class="menu-item"
              @click="selectOption('hospital')"
              :class="{ active: selectedOption === 'hospital', 'bold': selectedOption === 'hospital' }"
              style="pointer-events: auto;"
            >
          <div class="menu-content">
          <i class="icon">
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="23" viewBox="0 0 24 23" fill="none">
            <path d="M19.8771 17.7171H3.74402C3.51905 17.7171 3.33667 17.5347 3.33667 17.3097C3.33667 17.0847 3.519 16.9023 3.74402 16.9023H19.8771C20.1021 16.9023 20.2845 17.0847 20.2845 17.3097C20.2844 17.5347 20.1021 17.7171 19.8771 17.7171Z" fill="#707070"/>
            <path d="M21.6139 22.627H2.00731C1.10655 22.627 0.373779 21.8942 0.373779 20.9935C0.373779 20.4951 0.596873 20.0304 0.985928 19.7187L2.09789 18.8278C2.27371 18.687 2.5299 18.7153 2.67054 18.891C2.81119 19.0667 2.78288 19.323 2.60736 19.4637L1.4954 20.3545C1.30031 20.5107 1.18849 20.7437 1.18849 20.9935C1.18849 21.4451 1.55573 21.8123 2.00731 21.8123H21.6139C22.0654 21.8123 22.4327 21.4451 22.4327 20.9935C22.4327 20.7437 22.321 20.5107 22.1259 20.3545L21.014 19.4637C20.8383 19.323 20.81 19.0667 20.9506 18.891C21.0913 18.7152 21.3478 18.6873 21.5231 18.8278L22.6351 19.7186C23.0243 20.0303 23.2474 20.4949 23.2474 20.9935C23.2474 21.8942 22.5145 22.627 21.6139 22.627Z" fill="#707070"/>
            <path d="M18.2068 4.57228C17.9819 4.57228 17.7995 4.38996 17.7995 4.16493V2.15551C17.7995 1.62184 17.3654 1.18775 16.8317 1.18775H6.78975C6.25608 1.18775 5.82179 1.62184 5.82179 2.15551V3.82615C5.82179 4.05112 5.63946 4.2335 5.41443 4.2335C5.18941 4.2335 5.00708 4.05117 5.00708 3.82615V2.15551C5.00708 1.17261 5.80679 0.373047 6.7897 0.373047H16.8317C17.8144 0.373047 18.6141 1.17266 18.6141 2.15551V4.16493C18.6142 4.38991 18.4319 4.57228 18.2068 4.57228Z" fill="#707070"/>
            <path d="M19.8793 20.0917H3.74177C2.37983 20.0917 1.27173 18.9836 1.27173 17.6217V4.66731C1.27173 3.30542 2.37983 2.19727 3.74177 2.19727H5.41429C5.63927 2.19727 5.82164 2.37959 5.82164 2.60462V15.9343C5.82164 16.468 6.25589 16.9021 6.78961 16.9021H16.8316C17.3652 16.9021 17.7993 16.468 17.7993 15.9343V2.60462C17.7993 2.37964 17.9817 2.19727 18.2067 2.19727H19.8794C21.2411 2.19727 22.3492 3.30536 22.3492 4.66731V17.6217C22.3492 18.9835 21.241 20.0917 19.8793 20.0917ZM3.74177 3.01197C2.82907 3.01197 2.08644 3.7546 2.08644 4.66731V17.6217C2.08644 18.5344 2.82907 19.277 3.74177 19.277H19.8793C20.792 19.277 21.5345 18.5344 21.5345 17.6217V4.66731C21.5345 3.7546 20.792 3.01197 19.8793 3.01197H18.614V15.9343C18.614 16.9172 17.8142 17.7168 16.8315 17.7168H6.78955C5.80665 17.7168 5.00694 16.9172 5.00694 15.9343V3.01197H3.74177Z" fill="#707070"/>
            <path d="M12.5629 7.05286H11.0583C10.8333 7.05286 10.651 6.87053 10.651 6.64551V5.71049H9.71595C9.49097 5.71049 9.30859 5.52816 9.30859 5.30313V3.79856C9.30859 3.57353 9.49092 3.3912 9.71595 3.3912H10.651V2.45618C10.651 2.23116 10.8333 2.04883 11.0583 2.04883H12.5629C12.7879 2.04883 12.9703 2.23116 12.9703 2.45618V3.39115H13.9054C14.1304 3.39115 14.3128 3.57348 14.3128 3.79851V5.30313C14.3128 5.52811 14.1305 5.71049 13.9054 5.71049H12.9703V6.64551C12.9703 6.87053 12.7879 7.05286 12.5629 7.05286ZM11.4657 6.23815H12.1556V5.30318C12.1556 5.07816 12.3379 4.89583 12.5629 4.89583H13.4981V4.20591H12.5629C12.338 4.20591 12.1556 4.02358 12.1556 3.79856V2.86354H11.4657V3.79856C11.4657 4.02353 11.2833 4.20591 11.0583 4.20591H10.1234V4.89583H11.0583C11.2833 4.89583 11.4657 5.07816 11.4657 5.30318V6.23815Z" fill="#707070"/>
            <path d="M16.1155 10.4631H7.40369C7.17872 10.4631 6.99634 10.2808 6.99634 10.0558C6.99634 9.83077 7.17867 9.64844 7.40369 9.64844H16.1155C16.3406 9.64844 16.5229 9.83077 16.5229 10.0558C16.5229 10.2808 16.3406 10.4631 16.1155 10.4631Z" fill="#707070"/>
            <path d="M16.1155 12.1917H7.40369C7.17872 12.1917 6.99634 12.0093 6.99634 11.7843C6.99634 11.5593 7.17867 11.377 7.40369 11.377H16.1155C16.3406 11.377 16.5229 11.5593 16.5229 11.7843C16.5229 12.0093 16.3406 12.1917 16.1155 12.1917Z" fill="#707070"/>
            <path d="M16.1155 8.73463H7.40369C7.17872 8.73463 6.99634 8.55225 6.99634 8.32728C6.99634 8.1023 7.17867 7.91992 7.40369 7.91992H16.1155C16.3406 7.91992 16.5229 8.10225 16.5229 8.32728C16.5229 8.55225 16.3406 8.73463 16.1155 8.73463Z" fill="#707070"/>
            <path d="M16.1155 13.9202H7.40369C7.17872 13.9202 6.99634 13.7378 6.99634 13.5128C6.99634 13.2878 7.17867 13.1055 7.40369 13.1055H16.1155C16.3406 13.1055 16.5229 13.2878 16.5229 13.5128C16.5229 13.7378 16.3406 13.9202 16.1155 13.9202Z" fill="#707070"/>
            <path d="M16.1155 15.6487H7.40369C7.17872 15.6487 6.99634 15.4664 6.99634 15.2413C6.99634 15.0163 7.17867 14.834 7.40369 14.834H16.1155C16.3406 14.834 16.5229 15.0163 16.5229 15.2413C16.5229 15.4664 16.3406 15.6487 16.1155 15.6487Z" fill="#707070"/>
            </svg>
          </i>
          <div class="hospital-info">Hospital/Clinic Information</div>
          </div>
          <div class="arrow" v-if="selectedOption === 'hospital'">
            <svg xmlns="http://www.w3.org/2000/svg" width="17" height="17" viewBox="0 0 17 17" fill="none">
            <path fill-rule="evenodd" 
            clip-rule="evenodd" d="M6.08965 3.45437C6.29932 3.27465 6.61497 3.29893 6.79468 3.5086L10.7947 8.17526C10.9552 8.36251 10.9552 8.63881 10.7947 8.82606L6.79468 13.4927C6.61497 13.7024 6.29932 13.7267 6.08965 13.547C5.87999 13.3672 5.85571 13.0516 6.03542 12.8419L9.75651 8.50066L6.03542 4.15939C5.85571 3.94973 5.87999 3.63408 6.08965 3.45437Z" 
            fill="#707070"/>
            </svg>
          </div>
          </a>
        </div>
        <hr>
        <div class="doctor">
          <a
              class="menu-item"
              @click="selectOption('doctor')"
              :class="{ active: selectedOption === 'doctor', 'bold': selectedOption === 'doctor' }"
            >
          <div class="menu-content">  
          <i class="icon">
            <img
              :src="doctorInfoIcon"
              alt="Doctor Icon"
              class="doctor-info-icon"
            />
          </i>
          <div class="doctor-info">Doctor's Information</div>
          </div>
          <div class="arrow" v-if="selectedOption === 'doctor'">
            <svg xmlns="http://www.w3.org/2000/svg" width="17" height="17" viewBox="0 0 17 17" fill="none">
            <path fill-rule="evenodd" 
            clip-rule="evenodd" d="M6.08965 3.45437C6.29932 3.27465 6.61497 3.29893 6.79468 3.5086L10.7947 8.17526C10.9552 8.36251 10.9552 8.63881 10.7947 8.82606L6.79468 13.4927C6.61497 13.7024 6.29932 13.7267 6.08965 13.547C5.87999 13.3672 5.85571 13.0516 6.03542 12.8419L9.75651 8.50066L6.03542 4.15939C5.85571 3.94973 5.87999 3.63408 6.08965 3.45437Z" 
            fill="#707070"/>
            </svg>
          </div>
          </a>
        </div>
        <hr>
        <div class="legal">
          <a
              class="menu-item"
              @click="selectOption('legal')"
              :class="{ active: selectedOption === 'legal', 'bold': selectedOption === 'legal' }"
            >
           <div class="menu-content"> 
          <i class="icon">
            <svg xmlns="http://www.w3.org/2000/svg" width="22" height="23" viewBox="0 0 22 23" fill="none">
            <mask id="mask0_2525_1870" style="mask-type:luminance" maskUnits="userSpaceOnUse" x="0" y="0" width="22" height="23">
              <path d="M21.9785 22.1719V0.214846H0.0214844V22.1719H21.9785Z" fill="white" stroke="white" stroke-width="0.0429687"/>
            </mask>
            <g mask="url(#mask0_2525_1870)">
              <path d="M17.3553 21.7637H1.68624C1.11295 21.7637 0.644287 21.2936 0.644287 20.7211V4.01758C0.644287 3.44511 1.11295 2.97499 1.68624 2.97499H2.88029" stroke="#707070" stroke-width="0.866937" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
              <path d="M6.07236 19.4102H3.92156C3.34948 19.4102 2.88086 18.9412 2.88086 18.3676V1.66526C2.88086 1.09159 3.34948 0.622671 3.92156 0.622671H12.9532L17.3558 4.37406V12.2776M14.7729 19.4102H9.92967" stroke="#707070" stroke-width="0.866937" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
              <path d="M17.3555 4.375H14.2322C13.5286 4.375 12.9529 3.80012 12.9529 3.09612V0.623614" stroke="#707070" stroke-width="0.866937" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
              <path d="M5.52051 6.23438H14.714" stroke="#707070" stroke-width="0.866937" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
              <path d="M11.7185 8.39844H14.714M5.52051 8.39844H8.6901" stroke="#707070" stroke-width="0.866937" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
              <path d="M5.52051 10.5625H14.714" stroke="#707070" stroke-width="0.866937" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
              <path d="M8.55035 12.7266H12.1021M5.52075 12.7266H6.38619" stroke="#707070" stroke-width="0.866937" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
              <path d="M5.52026 14.8906H10.8126" stroke="#707070" stroke-width="0.866937" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
              <path d="M5.52051 4.06836H10.8202" stroke="#707070" stroke-width="0.866937" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
              <path d="M16.8844 21.5773C15.8473 20.9574 15.0038 19.9099 14.3538 18.6968C13.699 17.4751 13.3435 16.1853 13.3545 14.9685C13.3593 14.4289 13.4799 14.3693 13.9278 14.1281L17.3554 12.278L20.7831 14.1281C21.2298 14.3693 21.3503 14.4289 21.3551 14.9685C21.3661 16.1853 21.0107 17.4751 20.3559 18.6968C19.7059 19.9099 18.8624 20.9574 17.8253 21.5773C17.4102 21.8258 17.2995 21.8258 16.8844 21.5773Z" stroke="#707070" stroke-width="0.866937" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
              <path d="M7.99878 19.4102H8.00223" stroke="#707070" stroke-width="0.866937" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
              <path d="M16.0166 16.9562L16.8881 17.7734L18.6945 15.9184" stroke="#707070" stroke-width="0.866937" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"/>
            </g>
            </svg>
          </i>
          <div class="legal-info">Legal & Compliance Settings</div>
           </div>
          <div class="arrow" v-if="selectedOption === 'legal'">
            <svg xmlns="http://www.w3.org/2000/svg" width="17" height="17" viewBox="0 0 17 17" fill="none">
            <path fill-rule="evenodd" 
            clip-rule="evenodd" d="M6.08965 3.45437C6.29932 3.27465 6.61497 3.29893 6.79468 3.5086L10.7947 8.17526C10.9552 8.36251 10.9552 8.63881 10.7947 8.82606L6.79468 13.4927C6.61497 13.7024 6.29932 13.7267 6.08965 13.547C5.87999 13.3672 5.85571 13.0516 6.03542 12.8419L9.75651 8.50066L6.03542 4.15939C5.85571 3.94973 5.87999 3.63408 6.08965 3.45437Z" 
            fill="#707070"/>
            </svg>
          </div>
          </a>
        </div>
        </div>
        <div class="content-panel">
          <HospitalInformation v-if="selectedOption === 'hospital'" />
          <DoctorInformation v-if="selectedOption === 'doctor'" />
          <LegalCompliance v-if="selectedOption === 'legal'" />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted, onBeforeUnmount } from "vue";
import SideBar from "../components/SideBar.vue";
import ProfileDropDown from "@/components/ProfileDropDown.vue";
import Icon from "@/assets/doctor-icon.png";
import doctorInfoIcon from "@/assets/icons/doctor-info-icon.png";
import HospitalInformation from "@/Pages/modals/ClinicInfo.vue";
import DoctorInformation from "@/Pages/modals/DoctorInfo.vue";
import LegalCompliance from "@/Pages/modals/LegalCompliance.vue"

const image = ref("");
const menuOpen = ref(false);
const selectedOption = ref('hospital'); 
const showToast = ref(false);
const message = ref("");
const success = ref(false);

const selectOption = (option) => {
  selectedOption.value = option;
  showToast.value = false;
};
onMounted(() => {
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
  window.addEventListener("settings-updated", handleSettingsUpdated);

  document.addEventListener("click", handleClickOutside);
});
onUnmounted(() => {
  document.removeEventListener("click", handleClickOutside);
});
const toggleMenu = () => {
  menuOpen.value = !menuOpen.value;
};

onBeforeUnmount(() => {
  window.removeEventListener("settings-updated", handleSettingsUpdated);
});

function handleSettingsUpdated(event) {
  const { success: isSuccess, message: eventMessage } = event.detail;
  success.value = isSuccess;
  message.value = eventMessage;
  showToast.value = true;

   setTimeout(() => {
     showToast.value = false;
   }, 3000);
}

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
</script>

<style scoped>
.parent-container {
  height: 100vh;
  position: relative;
  overflow-y: auto;
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
.doctor-info-icon {
 width: 24px;
 height: 26px;
 filter: brightness(0)  invert(20%);
}

.frame {
  align-items: flex-start;
  border-radius: 12px;
  background: var(--bg-white, #fff);
  box-shadow: 0px 4px 14px -1px #e5f1ff;
  display: flex;
  flex-direction: column;
  gap: 10px;
  padding: 19px 30px;
  position: relative;
  width: 93%;
  margin-top: 10px;
  margin-left: 22px;
}

.frame .frame-wrapper {
  align-items: center;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  gap: 136px;
  position: relative;
  width: 100%;
}

.frame .div-wrapper {
  align-items: flex-start;
  display: inline-flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 20px;
  position: relative;
}

.frame .div {
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  gap: 30px;
  position: relative;
  width: 100%;
}

.frame .text-wrapper {
  color: #1c1c1c;
  font-size: 20px;
  font-weight: 600;
  letter-spacing: 0;
  line-height: normal;
  margin-top: -1px;
  position: relative;
  white-space: nowrap;
  width: fit-content;
}
.main-content{
  display: flex;
  flex-direction: column;
  background: #f3f9ff;
  margin-left: 80px;
  margin-top: 0;
}

.panelframe{
  display: flex;
  flex-direction: row;
}

.sidepanel{
  display: flex;
  width: 300px;
  height: auto;
  padding: 20px;
  flex-direction: column;
  align-items: flex-end;
  gap: 18px;
  flex-shrink: 0;
  border-radius: 12px;
  background: var(--bg-white, #FFF);
  margin-left: 22px;
  margin-top: 1%;
}
.hospital{
  display: flex;
  flex-direction: row;
  align-items: center;
}

.hospital-info{
  color: #1C1C1C;
  font-size: 16px;
  font-style: normal;
  font-weight: 400;
  line-height: normal;
  margin-left: 19px;
  cursor: pointer;
}
.doctor-info{
  color: #1C1C1C;
  font-size: 16px;
  font-style: normal;
  font-weight: 400;
  line-height: normal;
  cursor: pointer;
  margin-left: 19px;
}
.legal-info{
  color: #1C1C1C;
  font-size: 16px;
  font-style: normal;
  font-weight: 400;
  line-height: normal;
  margin-left: 19px;
  cursor: pointer;
}
.menu-item {
  display: flex;
  width: 298px;
  align-items: center;
  justify-content: space-between;
}
.arrow{
  margin-left: auto;
  display: flex;
  align-items: center;
}
hr {
  width: 296.002px;
  height: 1px;
  background: #E3E9F1;
  border: none;
  margin-top: 1px;
}
.content-panel{
display: flex;
width: 996px;
padding: 18px 21px;
flex-direction: column;
align-items: flex-start;
gap: 96px;
flex-shrink: 0;
border-radius: 12px;
background: #FFF;
margin-left: 23px;
margin-top: 16px;
}
.bold {
  font-weight: 700; 
}

.active .doctor-info,
.active .legal-info,
.active .hospital-info {
  font-weight: 700;
}

.toast {
  position: absolute;
  right: 34%;
  top: 14%;
  width: 22%;
}

.profile-updated-successfully {
  position: relative;
  line-height: 150%;
  width: 87%;
  font-size: 12px;
  color: #1c1c1c;
  flex-grow: 1;
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
  padding: 10px;
  border-radius: 4px;
  position: relative;
}

.bold-essentional-ui-check,
.bold-essentional-ui-error {
  width: 21px;
  height: 21px;
}

.image-delete-icon {
  border: 0;
  background: none;
  cursor: pointer;
  padding-top: 2px;
}

.menu-content {
  display: flex;
  align-items: center;
  gap: 0; 
}
</style>
