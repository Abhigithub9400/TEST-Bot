<template>
 <div class="website about-us-section">
  <section>
    <HeroSection
      title="Transforming Healthcare with AI-Driven Medical Scribe Solutions"
      :description="`${store.MediAssistConfigManager.DomainName} provides AI-powered medical transcription solutions that automate SOAP note creation, optimizing documentation for healthcare providers. We are dedicated to enhancing clinical workflows by reducing administrative burdens, allowing physicians to focus on patient care.`"
      :imageUrl="require('@/assets/AboutUsImages/FirstImage.jpg')"
    />
  </section>
 </div>
 <section id="aboutUs" class="aboutUs" ref="aboutUs" v-scroll-reveal>
  <div class="div">
    <div class="div-2">
    <div class="outline-security-eye-wrapper">
      <img alt="EyeImage" src="@/assets/AboutUsImages/EyeImage.svg"/>
    </div>
    <div class="div-3" v-scroll-reveal>
        <h2 class="text-wrapper h2-semibold">Our Vision</h2>
        <h7 class="p h7-regular">
        To enable healthcare professionals to spend more time with patients by minimizing the complexities of medical documentation, ultimately improving patient outcomes.
        </h7>
    </div>
    </div>
    <div class="div-2" v-scroll-reveal>
    <div class="outline-security-eye-wrapper">
      <img alt="EyeImage" src="@/assets/AboutUsImages/EyeImage.svg"/>
    </div>
    <div class="div-3">
        <h2 class="text-wrapper h2-semibold">Our Mission</h2>
        <h7 class="p h7-regular">
        To revolutionize healthcare documentation through AI-driven transcription technology, ensuring efficiency, accuracy, and seamless integration with clinical workflows.
        </h7>
    </div>
    </div>
  </div>
 </section>
 <section id="frame" class="frame" ref="frame">
    <div class="frame-wrapper">
      <div class="div">
        <img class="rectangle" alt="Rectangle" src="@/assets/AboutUsImages/Rectangle.jpg"/>

        <div class="div-wrapper">
          <div class="div-2">
            <div class="div-wrapper-2">
              <h3 class="text-wrapper h3-semibold">Why Choose Us?</h3>
            </div>
            <div class="div-3">
              <div class="frame-wrapper-6" v-scroll-reveal="{ origin: 'top', distance: '50px', delay: 300 }">
                <div class="div-4">
                  <div class="group-wrapper">
                    <img alt="AITechnology" src="@/assets/AboutUsImages/AITechnology.svg"/>
                  </div>

                  <div class="div-5">
                    <div class="div-wrapper-3">
                      <h5 class="text-wrapper-2 h5-medium ">
                        Industry-Leading AI Technology
                      </h5>
                    </div>

                    <p class="p lg-regular">
                      Advanced AI and machine learning algorithms ensure superior transcription accuracy.
                    </p>
                  </div>
                </div>
              </div>

              <div class="frame-wrapper-6" v-scroll-reveal="{ origin: 'top', distance: '50px', delay: 300 }">
                <div class="div-4">
                  <div class="group-wrapper">
                    <img alt="SeamlessIntegration" src="@/assets/AboutUsImages/Seamless.svg"/>
                  </div>

                  <div class="div-5">
                    <div class="div-wrapper-3">
                      <h5 class="text-wrapper-2 h5-medium">
                        Seamless Integration
                      </h5>
                    </div>

                    <p class="p lg-regular">
                      Compatible with existing EHR systems for smooth data transfer.
                    </p>
                  </div>
                </div>
              </div>

              <div class="frame-wrapper-6" v-scroll-reveal="{ origin: 'top', distance: '50px', delay: 300 }">
                <div class="div-4">
                  <div class="group-wrapper">
                    <img alt="EnhancedSecurity" src="@/assets/AboutUsImages/Enhanced.svg"/>
                  </div>

                  <div class="div-5">
                    <div class="div-wrapper-3">
                      <h5 class="text-wrapper-2 h5-medium">Enhanced Security</h5>
                    </div>

                    <p class="p lg-regular">
                      HIPAA-compliant data protection and automatic audio deletion post-processing.
                    </p>
                  </div>
                </div>
              </div>

              <div class="frame-wrapper-6" v-scroll-reveal="{ origin: 'top', distance: '50px', delay: 300 }">
                <div class="div-4">
                  <div class="group-wrapper">
                    <img alt="MultiDevice" src="@/assets/AboutUsImages/MultiDevice.svg"/>
                  </div>

                  <div class="div-5">
                    <div class="div-wrapper-3">
                      <h5 class="text-wrapper-2 h5-medium">
                        Multi-Device Accessibility
                      </h5>
                    </div>

                    <p class="p lg-regular">
                      Access from desktops, laptops, tablets, and smartphones.
                    </p>
                  </div>
                </div>
              </div>

              <div class="frame-wrapper-6" v-scroll-reveal="{ origin: 'top', distance: '50px', delay: 300 }">
                <div class="div-4">
                  <div class="group-wrapper">
                    <img alt="CustomizableWorkflows" src="@/assets/AboutUsImages/Customizable.svg"/>
                  </div>

                  <div class="div-5">
                    <div class="div-wrapper-3">
                      <h5 class="text-wrapper-2 h5-medium">
                        Customizable Workflows
                      </h5>
                    </div>

                    <p class="p lg-regular">
                      Tailor SOAP notes and templates to match specific medical specialties.
                    </p>
                  </div>
                </div>
              </div>
              <div class="frame-wrapper-8" v-scroll-reveal="{ origin: 'top', distance: '50px', delay: 300 }">
               <button @click="handleRequestDemo('request-demo')"  class="frame-wrapper-9">
                Book a Demo
               </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
 </section>
  <hr/>
</template>

<script setup>
import { useMyStore } from '@/store/store.ts';
import { useRouter } from 'vue-router';
import { ref } from "vue";
import HeroSection from "@/components/HeroSection.vue";

const router = useRouter();
const store = useMyStore();
const aboutUs = ref(null);
const frame = ref(null);
const handleRequestDemo = (modalType) => {
  localStorage.setItem('modalType', modalType);
  router.push('/request-demo');
};

</script>

<style scoped lang="css">
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
 }
 .about-us-section {
  width: 100%;
  position: relative;
  background: linear-gradient(0deg, #050613 0%, #051B2F 100%);
    
 }
 .about-us {
    width: 100%;
    height: 540px;
    padding: 0px 100px 0px 98px;
  }
  
  .firstImage {
    width: 414.5px;
    height: 414.5px;
    border-radius: 24px 24px 0px 0px;
    position: absolute;
    left: 1006px;
    top: 126.5px;
  }

  .about-us .text-wrapper {
    color: #ffffff;
    font-family: "Inter-SemiBold", Helvetica;
    font-size: 52px;
    font-weight: 550;
    left: 97px;
    letter-spacing: 0;
    line-height: 60px;
    position: absolute;
    top: 150px;
    width: 750px;
  }

  .about-us .p {
    align-self: stretch;
    color: #FFF;
    font-family: Inter;
    font-size: 16px;
    font-weight: 400;
    line-height: 24px;
    position: absolute;
    top: 350px;
    text-align: justify;
  }
  .aboutUs {
    width: 100%;
    height: fit-content;
    display: flex;
    padding: 80px 0;
    flex-direction: column;
    align-items: flex-start;
    gap: 10px;
    align-self: stretch;
    background: #F7FBFF;
  }

 .aboutUs .div {
  margin-inline: auto;
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  gap: 38px;
  position: relative;
  width: 1277px;
 }

 .aboutUs .div-2 {
  align-items: flex-start;
  display: flex;
  flex: 1;
  flex-grow: 1;
  gap: 32px;
  position: relative;
 }

 .aboutUs .outline-security-eye-wrapper {
  align-items: center;
  background-color: #ffffff;
  border-radius: 16px;
  display: inline-flex;
  flex: 0 0 auto;
  gap: 10px;
  position: relative;
}

 .aboutUs .div-3 {
  align-items: flex-start;
  display: flex;
  flex: 1;
  flex-direction: column;
  flex-grow: 1;
  gap: 16px;
  position: relative;
 }

 .aboutUs .text-wrapper {
  align-self: stretch;
  color: #1c1c1c;
  letter-spacing: 0;
  margin-top: -1.00px;
  position: relative;
 }

 .aboutUs .p {
  align-self: stretch;
  color: #303030;
  letter-spacing: 0;
  line-height: 150%;
  position: relative;
  text-align: justify;
 }
 .frame {
  align-items: center;
  background-color: #ffffff;
  display: flex;
  flex-direction: column;
  gap: 40px;
  justify-content: center;
  padding: 100px;
  position: relative;
 }
 .frame .frame-wrapper {
  align-items: flex-start;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 59.81px;
  margin-left: -0.50px;
  margin-right: -0.50px;
  position: relative;
  width: 1277px;
 }
 .frame .div {
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  justify-content: space-between;
  position: relative;
  width: 100%;
 }

 .frame .rectangle {
  align-self: stretch;
  position: relative;
  width: 502.4px;
  height: 680px;
 }
 .frame .div-wrapper {
  position: relative;
  width: 713px;
 }

 .frame .div-2 {
  align-items: flex-start;
  display: flex;
  flex-direction: column;
  gap: 40px;
  position: relative;
 }
 .frame .div-wrapper-2 {
  align-items: center;
  display: flex;
  flex: 1;
  flex-direction: column;
  gap: 31.9px;
  justify-content: center;
  position: relative;
 }
 .frame .text-wrapper {
  align-self: stretch;
  color: #181818;
  letter-spacing: 0;
  line-height: 57.8px;
  margin-top: -1.00px;
  position: relative
 }
 .frame .div-3{
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 32px;
  position: relative;
  width: 100%;
 }
 .frame .frame-wrapper-6{
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  gap: 24px;
  position: relative;
  width: 100%;
 }
 .frame .div-4{
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  gap: 24px;
  position: relative;
  width: 100%;
 }
 .frame .group-wrapper{
  align-items: center;
  background-color: #f7fbff;
  border-radius: 8px;
  display: inline-flex;
  flex: 0 0 auto;
  position: relative;
 }
 .frame .div-5{
  align-items: flex-start;
  display: flex;
  flex: 1;
  flex-direction: column;
  flex-grow: 1;
  gap: 10px;
  position: relative;
 }
 .frame .div-wrapper-3{
  align-items: flex-start;
  display: inline-flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 9.74px;
  position: relative;
 }
 .frame .text-wrapper-2{
  align-self: stretch;
  color: #000000;
  letter-spacing: 0;
  margin-top: -0.65px;
  position: relative;
 }
 .frame .p{
  align-self: stretch;
  color: #646363;
  letter-spacing: 0;
  position: relative;
 }
 .frame .frame-wrapper-8{
  padding: 0px 90px;
  position: relative;
 }
 .frame .frame-wrapper-9{
  align-items: center;
  background: linear-gradient(
    310deg,
    rgba(0, 102, 212, 1) 0%,
    rgba(0, 53, 110, 1) 100%
  );
  border: none;
  border-radius: 7.82px;
  display: inline-flex;
  flex-direction: column;
  height: 40px;
  justify-content: center;
  padding: 16px;
  position: relative;
  color: #ffffff;
  font-family: "Inter-Medium", Helvetica;
  font-size: 16px;
  font-weight: 500;
  line-height: 24px;
  width: 140px;
 }
 .frame .frame-wrapper-9:hover{
  background: linear-gradient(0deg, #2D4F9F, #2D4F9F),
  linear-gradient(274.4deg, #0057B4 2.36%, #00254D 97.81%);
  cursor: pointer;
  font-weight: bold;
 }
hr{
    height: 0.03125rem;
    background-color: rgba(10, 33, 68, 0.15);
    border: none;
}
</style>