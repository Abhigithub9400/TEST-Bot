<template>
  <div class="website">
    <div class="license-container" v-scroll-reveal>
      <section class="license-header" id="license-header" ref="license-header">
        <h1 class="license-text h1-semibold">License Agreement</h1>
      </section>
    </div>
    <div
      class="license-frame"
      id="licenseFrame"
      ref="licenseFrame"
      v-scroll-reveal
    >
      <div class="license-frame-wrapper">
        <div class="license-div">
          <div class="license-div-2">
            <h6 class="license-text-wrapper h6-medium">License Grant</h6>
            <div class="license-p lg-regular">
              PIT Solutions Pvt. Ltd. grants users a non-transferable,
              non-exclusive license to access and use
              {{ store.MediAssistConfigManager.DomainName }} AI solely within
              authorized healthcare institutions in India and the UAE. This
              license is strictly for professional healthcare settings, such as
              hospitals and clinics. Individual practitioner use is subject to
              future terms.
            </div>
          </div>
          <div class="license-div-2">
            <h6 class="license-text-wrapper h6-medium">License Restrictions</h6>
            <div class="license-p lg-regular">
              <ul>
                <li>
                  <span
                    >Users shall not reverse-engineer, decompile, or disassemble
                    the software.</span
                  >
                </li>
                <li>
                  <span
                    >Resale, distribution, or modification of the application
                    without authorization is prohibited.</span
                  >
                </li>
                <li>
                  <span
                    >PIT Solutions Pvt. Ltd. reserves the right to audit usage
                    to ensure compliance with license terms.</span
                  >
                </li>
              </ul>
            </div>
          </div>
          <div class="license-div-2">
            <h6 class="license-text-wrapper h6-medium">
              Intellectual Property Ownership
            </h6>
            <div class="license-p lg-regular">
              All proprietary rights within
              {{ store.MediAssistConfigManager.DomainName }} AI, including AI
              models, transcription software, and data structures, are owned by
              PIT Solutions Pvt. Ltd.. Users retain ownership over their
              respective data.
            </div>
          </div>
          <div class="license-div-2">
            <h6 class="license-text-wrapper h6-medium">
              Liability and Warranty Limitations
            </h6>
            <div class="license-p lg-regular">
              {{ store.MediAssistConfigManager.DomainName }} AI is provided on
              an "as is" basis, and PIT Solutions Pvt. Ltd. is not liable for
              any indirect, incidental, or consequential damages resulting from
              the application’s use, including diagnostic accuracy or
              transcription outcomes.
            </div>
          </div>
          <div class="license-div-2">
            <h6 class="license-text-wrapper h6-medium">Termination</h6>
            <div class="license-p lg-regular">
              This License Agreement remains effective until terminated.
              Violating any terms may result in immediate revocation of the
              license. Upon termination, users must cease access and use of the
              software and delete or return any application materials as
              required.
            </div>
          </div>
          <div class="license-div-2">
            <h6 class="license-text-wrapper h6-medium">
              End-User Data Ownership
            </h6>
            <div class="license-p lg-regular">
              End-users retain ownership over patient data and other medical
              information, with PIT Solutions Pvt. Ltd. serving solely as a data
              processor.
            </div>
          </div>
          <div class="license-div-2">
            <h6 class="license-text-wrapper h6-medium">
              Post-Termination Obligations
            </h6>
            <div class="license-p lg-regular">
              Users must delete all access credentials and cease using the
              application upon termination of this license.
            </div>
          </div>
          <div class="license-div-2">
            <h6 class="license-text-wrapper h6-medium">Jurisdiction</h6>
            <div class="license-p lg-regular">
              This agreement is governed by the laws of India and the UAE.
              Disputes shall be resolved in the relevant jurisdiction.
            </div>
          </div>
          <div class="license-div-2">
            <h6 class="license-text-wrapper h6-medium">Entire Agreement</h6>
            <div class="license-p lg-regular">
              This License Agreement represents the complete agreement between
              the user and PIT Solutions Pvt. Ltd. regarding
              {{ store.MediAssistConfigManager.DomainName }} AI.
            </div>
          </div>
          <div class="license-div-2">
            <h6 class="license-text-wrapper h6-medium">Contact Information</h6>
            <div class="license-p lg-regular">
              For license-related inquiries, please contact
              <a :href="`mailto:${store.MediAssistConfigManager.SupportEmail}`"
                >Email Us</a
              >.
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useMyStore } from "@/store/store.ts";
import { ref } from "vue";

const store = useMyStore();
const licenseFrame = ref(null);
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Karma:wght@400;500&display=swap");

.license-container {
  width: 100%;
  position: relative;
  background: linear-gradient(0deg, #050613 0%, #051b2f 100%);
}
.license-header {
  width: 100%;
  height: 230px;
}
.license-text {
  color: #ffffff;
  letter-spacing: 0;
  top: 125px;
  position: relative;
  width: 1277px;
  margin: 0px;
  margin-inline: auto;
}
.license-frame {
  align-items: flex-start;
  background-color: #f7fbff;
  display: flex;
  flex-direction: column;
  gap: 10px;
  padding: 60px 120px;
  position: relative;
}
.license-frame-wrapper {
  align-items: center;
  align-self: stretch;
  background-color: #ffffff;
  display: flex;
  flex: 0 0 auto;
  gap: 10px;
  justify-content: center;
  padding: 24px;
  position: relative;
  margin-inline: auto;
  width: 1277px;
}
.license-div {
  align-items: flex-start;
  display: flex;
  flex: 1;
  flex-direction: column;
  flex-grow: 1;
  gap: 25px;
  position: relative;
}
.license-div-2 {
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 10px;
  position: relative;
}
.license-text-wrapper {
  align-self: stretch;
  color: #1c1c1c;
  letter-spacing: 0;
  margin-top: -1px;
  position: relative;
}

.license-p {
  align-self: stretch;
  color: #1c1c1c;
  letter-spacing: 0;
  position: relative;
}

.license-p a {
  color: #0066d4;
  text-decoration-line: underline;
  text-underline-offset: auto;
  text-underline-position: from-font;
  text-decoration-style: solid;
  text-decoration-skip-ink: none;
  text-decoration-thickness: auto;
}
</style>
