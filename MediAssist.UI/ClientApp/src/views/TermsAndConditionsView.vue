<template>
  <div class="website">
    <div class="terms-container" v-scroll-reveal>
      <section class="terms-header" id="terms-header" ref="terms-header">
        <h1 class="terms-text h1-semibold">Terms and Conditions</h1>
      </section>
    </div>
    <div class="terms-frame" id="termsFrame" ref="termsFrame" v-scroll-reveal>
      <div class="terms-frame-wrapper">
        <div class="terms-div">
          <h4 class="terms-text-wrapper h4-semibold">
            Effective Date: 1st January 2025
          </h4>
          <div class="terms-div-2">
            <h6 class="terms-text-wrapper-2 h6-medium">Acceptance of Terms</h6>
            <div class="terms-p lg-regular">
              By accessing or using the
              {{ store.MediAssistConfigManager.DomainName }} AI application, you
              agree to comply with and be bound by these Terms and Conditions.
              If you disagree with any part of these terms, please refrain from
              using the application.
            </div>
          </div>
          <div class="terms-div-2">
            <h6 class="terms-text-wrapper-2 h6-medium">
              User Roles and Obligations
            </h6>
            <div class="terms-p lg-regular">
                <ul>
                    <li>
                        <span><b>Roles : </b></span>
                        By accessing or using the
                        {{ store.MediAssistConfigManager.DomainName }} AI application,
                        you agree to comply with and be bound by these Terms and
                        Conditions. If you disagree with any part of these terms,
                        please refrain from using the application.
                    </li>
                    <li>
                        <span><b>Obligations : </b></span>
                        Users must provide accurate registration information, protect
                        their credentials, and use the system only for its intended
                        healthcare purposes. Unauthorized access or any activities
                        that may harm the application or violate medical ethics are
                        strictly prohibited.
                    </li>
                    <li>
                        <span><b>Compliance : </b></span>
                        Users are responsible for adhering to all relevant healthcare regulations 
                        and ensuring their usage aligns with ethical and legal standards.
                        In addition, Users must comply with these terms and conditions.
                    </li>
                    <li>
                        <span><b>Qualification : </b></span>
                        Users must have adequate qualifications, certifications, licences, and other appropriate
                        permits issued by competent legal authorities in order to use  {{ store.MediAssistConfigManager.DomainName }}.
                        Specifically, since the Users are dealing with patients and their personal
                        information, they must be registered medical practitioners and they must use
                        {{ store.MediAssistConfigManager.DomainName }} only for the purpose specified herein.
                    </li>
                </ul>
            </div>
          </div>
          <div class="terms-div-2">
            <h6 class="terms-text-wrapper-2 h6-medium">Account Registration</h6>
            <div class="terms-p lg-regular">
              Account creation requires acceptance of the Privacy Policy and
              Terms. {{ store.MediAssistConfigManager.DomainName }} AI reserves
              the right to suspend accounts for violations or suspected
              fraudulent activity. By creating an account, users consent to the
              collection and processing of their data as outlined in the Privacy
              Policy.
            </div>
          </div>
          <div class="terms-div-2">
            <h6 class="terms-text-wrapper-2 h6-medium">
              Modification of Services
            </h6>
            <div class="terms-p lg-regular">
              {{ store.MediAssistConfigManager.DomainName }} AI reserves the
              right to modify, suspend, or discontinue any service or feature at
              any time without prior notice, including any functionality or AI
              capabilities. Continued use of the application implies acceptance
              of such modifications.
            </div>
          </div>
          <div class="terms-div-2">
            <h6 class="terms-text-wrapper-2 h6-medium">Prohibited Use</h6>
            <div class="terms-p lg-regular">
              Users shall not use
              {{ store.MediAssistConfigManager.DomainName }} AI for unlawful
              purposes, unauthorized data access, or attempts to disrupt
              application functionality. Unauthorized data mining, reverse
              engineering, or any actions that compromise application security
              or patient confidentiality are prohibited.
            </div>
          </div>
          <div class="terms-div-2">
            <h6 class="terms-text-wrapper-2 h6-medium">
              Third-Party Integrations Disclaimer
            </h6>
            <div class="terms-p lg-regular">
              If third-party tools are integrated within
              {{ store.MediAssistConfigManager.DomainName }} AI, be aware that
              these tools may access certain user data or functionality.
              {{ store.MediAssistConfigManager.DomainName }} AI does not control
              these third-party services, and users are advised to review
              relevant third-party terms.
            </div>
          </div>
          <div class="terms-div-2">
              <h6 class="terms-text-wrapper-2 h6-medium">
                  AI Support and Disclaimer
              </h6>
              <div class="terms-p lg-regular">
                  {{ store.MediAssistConfigManager.DomainName }} AI is designed as a
                  supporting tool for healthcare and medical practices. While it
                  provides transcription, diagnostic assistance, and potential
                  diagnosis suggestions, it is not a substitute for professional
                  medical advice or clinical decision-making. Users are solely
                  responsible for all clinical decisions.
              </div>
              <div class="terms-p lg-regular">
                  The licensor herein is not responsible or liable in any manner to
                  any one if the User uses  {{ store.MediAssistConfigManager.DomainName }} for any purpose other than the
                  purpose specified herein or for any negligence or intentional act
                  of the User.
              </div>
          </div>
          <div class="terms-div-2">
            <h6 class="terms-text-wrapper-2 h6-medium">
              Intellectual Property
            </h6>
            <div class="terms-p lg-regular">
              All content, including but not limited to text, graphics, and
              software within {{ store.MediAssistConfigManager.DomainName }} AI,
              is owned by PIT Solutions Pvt. Ltd. and protected by intellectual
              property laws. Unauthorized use, reproduction, or distribution is
              strictly prohibited.
            </div>
          </div>
          <div class="terms-div-2">
            <h6 class="terms-text-wrapper-2 h6-medium">
              Limitation of Liability
            </h6>
            <div class="terms-p lg-regular">
              {{ store.MediAssistConfigManager.DomainName }} AI is provided "as
              is" without warranties of accuracy, reliability, or availability.
              PIT Solutions Pvt. Ltd. shall not be held liable for any damages
              arising from the use or inability to use the application,
              particularly concerning diagnostic or transcription-related
              inaccuracies.
            </div>
          </div>
          <div class="terms-div-2">
            <h6 class="terms-text-wrapper-2 h6-medium">Jurisdiction</h6>
            <div class="terms-p lg-regular">
              This agreement is governed by the laws of India and the UAE. Legal
              disputes shall be resolved under the jurisdiction of the courts of
              India or the UAE, as applicable.
            </div>
          </div>
          <div class="terms-div-2">
            <h6 class="terms-text-wrapper-2 h6-medium">Changes to Terms</h6>
            <div class="terms-p lg-regular">
              {{ store.MediAssistConfigManager.DomainName }} AI reserves the
              right to amend these Terms and Conditions at any time. Continued
              use of the application after updates implies acceptance of the
              revised terms.
            </div>
          </div>
          <div class="terms-div-2">
            <h6 class="terms-text-wrapper-2 h6-medium">Contact Information</h6>
            <div class="terms-p lg-regular">
              For questions or concerns, please contact
              <a
                :href="`mailto:${store.MediAssistConfigManager.SupportEmail}`"
                >{{ store.MediAssistConfigManager.SupportEmail }}</a
              >
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
const termsFrame = ref(null);
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Karma:wght@400;500&display=swap");

.terms-container {
  width: 100%;
  position: relative;
  background: linear-gradient(0deg, #050613 0%, #051b2f 100%);
}
.terms-header {
  width: 100%;
  height: 230px;
}
.terms-text {
  color: #ffffff;
  letter-spacing: 0;
  top: 125px;
  position: relative;
  margin: 0px;
  margin-inline: auto;
  width: 1277px;
}
.terms-frame {
  align-items: flex-start;
  background-color: #f7fbff;
  display: flex;
  flex-direction: column;
  gap: 10px;
  padding: 60px 120px;
  position: relative;
}
.terms-frame-wrapper {
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
.terms-div {
  align-items: flex-start;
  display: flex;
  flex: 1;
  flex-direction: column;
  flex-grow: 1;
  gap: 25px;
  position: relative;
}
.terms-text-wrapper {
  align-self: stretch;
  color: #1c1c1c;
  letter-spacing: 0;
  margin-top: -1px;
  margin-bottom: -1px;
  position: relative;
}

.terms-div-2 {
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 10px;
  position: relative;
}
.terms-text-wrapper-2 {
  align-self: stretch;
  color: #1c1c1c;
  letter-spacing: 0;
  margin-top: -1px;
  position: relative;
}

.terms-p {
  align-self: stretch;
  color: #1c1c1c;
  letter-spacing: 0;
  position: relative;
}
.terms-p a {
  color: #0066d4;
  text-decoration-line: underline;
  text-underline-offset: auto;
  text-underline-position: from-font;
  text-decoration-style: solid;
  text-decoration-skip-ink: none;
  text-decoration-thickness: auto;
}
</style>
