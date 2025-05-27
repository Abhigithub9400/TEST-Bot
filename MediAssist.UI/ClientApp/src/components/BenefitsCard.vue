<template>
  <div class="benefits-card">
    <div class="frame-wrapper">
      <div class="div-wrapper">
        <div class="div">
          <h6 class="text-wrapper h6-medium" v-scroll-reveal>Key Benefits of {{store.MediAssistConfigManager.DomainName}}</h6>

          <div class="frame-wrapper-2">
            <div class="div-2">
              <div class="div-3">
                <div class="div-4">
                  <img class="rectangle" alt="Rectangle" :src="rectangle51110" v-scroll-reveal="{ distance: '50px', delay: 500 }"/>

                  <div class="div-5">
                    <h6 class="optimized-for h6-medium" v-scroll-reveal="{ distance: '50px', delay: 600 }">
                      Optimized for Doctors &amp; Clinics
                    </h6>

                    <p class="p" v-scroll-reveal="{ distance: '50px', delay: 700 }">
                      Built for independent practitioners, clinics &amp;
                      hospitals.
                    </p>
                  </div>
                </div>

                <div class="div-4">
                  <img class="rectangle" alt="Rectangle" :src="rectangle51111" v-scroll-reveal="{ distance: '50px', delay: 500 }"/>

                  <div class="div-5">
                    <h6 class="text-wrapper-2 h6-medium" v-scroll-reveal="{ distance: '50px', delay: 600 }">
                      Enhanced Documentation Efficiency
                    </h6>

                    <p class="p" v-scroll-reveal="{ distance: '50px', delay: 700 }">
                      AI-driven transcription reduces time spent on manual data entry.
                    </p>
                  </div>
                </div>
              </div>

              <div class="div-3">
                <div class="div-4">
                  <img class="rectangle" alt="Rectangle" :src="rectangle51112" v-scroll-reveal="{ distance: '50px', delay: 500 }"/>

                  <div class="div-5">
                    <h6 class="text-wrapper-2 h6-medium" v-scroll-reveal="{ distance: '50px', delay: 600 }">
                      Comprehensive Medical Terminology
                    </h6>

                    <p class="p" v-scroll-reveal="{ distance: '50px', delay: 700 }">
                      Robust database ensures accuracy in transcriptions and records.
                    </p>
                  </div>
                </div>

                <div class="div-4">
                  <img class="img" alt="Rectangle" :src="rectangle51113" v-scroll-reveal="{ distance: '50px', delay: 500 }">

                  <div class="div-5">
                    <h6 class="text-wrapper-3 h6-medium" v-scroll-reveal="{ distance: '50px', delay: 600 }">
                      Seamless EHR Integration
                    </h6>

                    <p class="p" v-scroll-reveal="{ distance: '50px', delay: 700 }">
                      Effortlessly integrates with existing Electronic Health Record (EHR) systems.
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import rectangle51110 from "@/assets/HomePageImages/Benefits/doctor-clinic-optimized.jpg";
import rectangle51111 from "@/assets/HomePageImages/Benefits/ai-enhanced-efficiency.jpg";
import rectangle51112 from "@/assets/HomePageImages/Benefits/healthcare-provider-focused.jpg";
import rectangle51113 from "@/assets/HomePageImages/Benefits/ehr-integration-seamless.jpg";
import { useMyStore } from '@/store/store.ts';

const store = useMyStore();
</script>

<style>
.benefits-card {
  padding: 40px;
  align-items: flex-start;
  background-size: cover;
  display: flex;
  flex-direction: column;
  gap: 10px;
  position: relative;
}

.benefits-card .mask-group {
  height: 394px;
  left: 0;
  position: absolute;
  top: 0;
  transform: rotate(-180.00deg);
  width: 496px;
}

.benefits-card .frame-wrapper {
  align-items: flex-start;
  align-self: stretch;
  box-shadow: 0px 4px 4px #00000040;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 32px;
  position: relative;
  padding: 80px;
  height: 100%;
}

.benefits-card .div-wrapper {
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 38px;
  position: relative;
  width: fit-content;
  margin-inline: auto;
}

.benefits-card .div {
  align-items: center;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 40px;
  position: relative;
  width: 1280px;
}

.benefits-card .text-wrapper {
  align-self: stretch;
  color: #dfdfdf;
  font-family: "Inter-SemiBold", Helvetica;
  font-size: 38px;
  font-weight: 600;
  letter-spacing: 0;
  line-height: 58px;
  margin-top: -1.00px;
  position: relative;
}

.benefits-card .frame-wrapper-2 {
  align-items: center;
  display: flex;
  flex-direction: column;
  gap: 24px;
  height: 426px;
  justify-content: center;
  position: relative;
  width: 100%;
}

.benefits-card .div-2 {
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  gap: 24px;
  position: relative;
  width: 100%;
}

.benefits-card .div-3 {
  align-items: center;
  display: flex;
  flex: 1;
  flex-grow: 1;
  gap: 24px;
  position: relative;
}

.benefits-card .div-4 {
  align-items: flex-start;
  display: flex;
  flex: 1;
  flex-direction: column;
  flex-grow: 1;
  gap: 24px;
  position: relative;
}

.benefits-card .rectangle {
  align-self: stretch;
  height: 290px;
  object-fit: cover;
  position: relative;
  width: 100%;
}

.benefits-card .div-5 {
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 8px;
  position: relative;
  width: 100%;
}

.benefits-card .optimized-for {
  color: #dfdfdf;
  margin: 0px;
  position: relative;
  width: 226.77px;
}

.benefits-card .p {
  color: #DFDFDF;
  font-size: var(--Fontsize-body-md, 14px);
  font-style: normal;
  font-weight: 400;
  line-height: var(--Lineheight-body-md, 20px);
  align-self: stretch;
  color: #DFDFDF;
  position: relative;
}

.benefits-card .text-wrapper-2 {
  color: #dfdfdf;
  margin-top: -1.00px;
  position: relative;
  width: 249.31px;
}

.benefits-card .img {
  align-self: stretch;
  height: 290px;
  position: relative;
  width: 100%;
}

.benefits-card .text-wrapper-3 {
  color: #dfdfdf;
  margin-top: -1.00px;
  position: relative;
  width: 223.87px;
}
</style>