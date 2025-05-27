<template>
  <div class="website">
    <div class="privacy-page" v-scroll-reveal>
      <section class="privacy-header" id="privacy-header" ref="privacy-header">
        <h1 class="privacy-text h1-semibold">Privacy Policy</h1>
      </section>
    </div>
    <div
      class="privacy-frame"
      id="privacyFrame"
      ref="privacyFrame"
      v-scroll-reveal
    >
      <div class="privacy-frame-wrapper">
        <div class="privacy-div">
          <h4 class="privacy-text-wrapper h4-semibold">
            Effective Date: 1st January 2025
          </h4>
          <div class="privacy-div-2">
            <h6 class="privacy-text-wrapper-2 h6-medium">Introduction</h6>
            <div class="privacy-p lg-regular">
              We at PIT Solutions Private Limited (together with our affiliates,
              “PitSolutions”, “we”, “our” or “us”), respect and take the privacy
              of individuals seriously, and are committed to keeping secure the
              personal data that we obtain from the users and other data
              subjects. This Privacy Policy outlines how we collect, use, and
              store personal data and the recorded conversations between the
              doctors and patients through our application. By using the
              {{ store.MediAssistConfigManager.DomainName }} service, you agree
              to the terms and conditions set forth in this Privacy Policy and
              if you are a doctor, you must convey the contents herein to the
              patient and get explicit approval from the patient as provided
              herein.
            </div>
          </div>
          <div class="privacy-div-2">
            <h6 class="privacy-text-wrapper-2 h6-medium">
              Information we collect
            </h6>
            <div class="privacy-p lg-regular">
              To provide our services, we may collect the following types of
              information :
              <ul>
                <li>
                  <span><b>Personal Identification Information :</b></span>
                  This includes information such as your name, age, gender, body
                  vitals, information about the ailment for which the patient is
                  consulting the doctor (symptoms, previous history, allergies,
                  medications, and all other matters that a patient discloses to
                  the doctor), doctor’s observations and prescriptions, voice,
                  and other information that can identify the patient as an
                  individual.
                </li>
                <li>
                  <span
                    ><b>Doctor-Patient Conversation (Voice & Data) :</b></span
                  >
                  Our application records voice conversations between the doctor
                  and the patient during consultations. This audio data may
                  include the personal identification information specified
                  above if they are spoken and other sensitive health
                  information.
                </li>
                <li>
                  <span><b>Usage Data :</b></span>
                  We collect data about how the app is used, such as device
                  type, operating system, IP address, location, in-app activity,
                  and other related technical and analytical information to
                  improve the service and troubleshoot technical issues.
                </li>
                <li>
                  <span><b>Log Data :</b></span>
                  We collect information that your browser or device
                  automatically sends when you use our Services. Log data
                  includes your Internet Protocol address, browser type and
                  settings, the date and time of your request, and how you
                  interact with our Services.
                </li>
                <li>
                  <span><b>Device Information :</b></span>
                  We collect information about the device you use to access the
                  Services, such as the name of the device, operating system,
                  device type, device identifiers, browser you are using, and
                  crash data. Information collected may include additional
                  information in respect of the device depending on the type of
                  device you use and its settings.
                </li>
                <li>
                  <span><b>Communication Information :</b></span>
                  If you communicate with us, such as via email or our pages on
                  social media sites, we may collect Personal Data like your
                  name, contact information, and the contents of the messages
                  you send.
                </li>
                <li>
                  <span><b>Other Information You Provide :</b></span>
                  We collect other information that you may provide to us, such
                  as when you participate in our events or surveys or provide us
                  with information to establish your identity or age.
                </li>
                <li>
                  <span><b>Usage Data :</b></span>
                  We collect information about your use of the Services, such as
                  the types of content that you view or engage with, the
                  features you use and the actions you take, as well as your
                  time zone, country, the dates and times of access, user agent
                  and version, type of computer or mobile device, and your
                  computer connection.
                </li>
                <li>
                  <span><b>Cookies and Similar Technologies :</b></span>
                  Cookies are text files containing small amounts of information
                  that may be placed on your device when you use
                  {{ store.MediAssistConfigManager.DomainName }}. We use cookies
                  and similar technologies to manage user sessions and
                  administer our Services and to improve your experience. The
                  cookies that we use functions in the following manner: once a
                  user logs in, a cookie is set to persist their login details,
                  even if the browser is closed. The cookie is cleared only when
                  the user logs out or when the configured time expires.
                </li>
              </ul>
            </div>
          </div>
          <div class="privacy-div-2">
            <h6 class="privacy-text-wrapper-2 h6-medium">
              Third party applications that we use to process the personal
              information
            </h6>
            <div class="privacy-p lg-regular">
              We use the following third-party artificial intelligence
              applications for processing your personal information and the
              processing by each of such third-party application is subject to
              their respective privacy policies. Please click the link provided
              below to access and read the respective privacy policies and if
              you or the patient does not agree with the terms therein, please
              do not use {{ store.MediAssistConfigManager.DomainName }}.
              <ul>
                <li>
                  <a href="https://openai.com/enterprise-privacy/">Open AI</a>
                </li>
                <li>
                  <a
                    href="https://learn.microsoft.com/en-us/legal/cognitive-services/openai/data-privacy?tabs=azure-portal"
                    >Microsoft Cognitive Services</a
                  >
                </li>
                <li>
                  <a
                    href="https://learn.microsoft.com/en-us/legal/cognitive-services/speech-service/speech-to-text/data-privacy-security?tabs=custom-neural-voice"
                    >Microsoft Speech-to-Text Data</a
                  >
                </li>
              </ul>
            </div>
          </div>
          <div class="privacy-div-2">
            <h6 class="privacy-text-wrapper-2 h6-medium">
              How we use personal information
            </h6>
            <div class="privacy-p lg-regular">
              We collect and use your personal information and conversation
              recordings for the following purposes:
              <ul>
                <li>
                  <span><b>Providing the Service :</b></span>
                  This is in the form of offering the option to download or
                  share the summary report or prescription generated from the
                  doctor-patient conversation.
                </li>
                <li>
                  <span><b> Improving the Service :</b></span>
                  To analyze usage data and improve the functionality and user
                  experience of the app.
                </li>
                <li>
                  <span
                    ><b
                      >Compliance with Legal and Regulatory Requirements :</b
                    ></span
                  >
                  To comply with relevant healthcare privacy laws and other
                  legal obligations.
                </li>
                <li>
                  <span><b>Security and Maintenance :</b></span>
                  To ensure the app is secure, functional, and free from bugs or
                  vulnerabilities.
                </li>
              </ul>
            </div>
          </div>
          <div class="privacy-div-2">
            <h6 class="privacy-text-wrapper-2 h6-medium">
              Consent for Recording Conversations
            </h6>
            <div class="privacy-p lg-regular">
              By using {{ store.MediAssistConfigManager.DomainName }}, it shall
              be deemed that both the doctor and the patient have provided their
              consent for processing their personal information as processed by
              the {{ store.MediAssistConfigManager.DomainName }} and as per the
              terms herein. <br />
              By using our app, you acknowledge that conversations may be
              recorded and stored. You may withdraw consent at any time by
              discontinuing use of the app or contacting us.
            </div>
          </div>
          <div class="privacy-div-2">
            <h6 class="privacy-text-wrapper-2 h6-medium">
              Data Storage and Retention
            </h6>
            <div class="privacy-p lg-regular">
              <ul>
                <li>
                  <span><b>Storage :</b></span>
                  We do not store the recorded conversations, summary, or the
                  prescriptions other than generation of the summary and the
                  prescriptions and sharing it with the doctor.
                </li>
                <li>
                  <span><b>Retention Period :</b></span>
                  The personal data that are provided into
                  {{ store.MediAssistConfigManager.DomainName }} are retained as
                  long as the user continues to use the
                  {{ store.MediAssistConfigManager.DomainName }} unless required
                  by law or for any other business reasons to be kept longer.
                </li>
                <li>
                  <span><b>Deletion :</b></span>
                  The data subjects may request the deletion of their personal
                  data at any time by contacting us at
                  <a
                    :href="`mailto:${store.MediAssistConfigManager.SupportEmail}`"
                    >{{ store.MediAssistConfigManager.SupportEmail }}</a
                  >. Upon receiving request, we will delete the personal data as
                  soon as possible, unless it is necessary for legal,
                  regulatory, or operational purposes.
                </li>
              </ul>
            </div>
          </div>
          <div class="privacy-div-2">
            <h6 class="privacy-text-wrapper-2 h6-medium">Sharing of Data</h6>
            <div class="privacy-p lg-regular">
              We do not sell, rent, or trade your personal information. However,
              we may share your data under the following circumstances:
              <ul>
                <li>
                  <span><b>With third-party processors :</b></span>
                  as provided in Section 2.
                </li>
                <li>
                  <span><b>With Healthcare Providers :</b></span>
                  Recorded conversations may be shared with healthcare providers
                  involved in your care.
                </li>
                <li>
                  <span><b>With Service Providers :</b></span>
                  We may engage third-party companies to provide certain
                  services (e.g., hosting, analytics, customer support). These
                  service providers are bound by confidentiality agreements and
                  are prohibited from using your data for any other purposes.
                </li>
                <li>
                  <span><b>As Required by Law :</b></span>
                  We may disclose your information if required by law,
                  regulation, legal process, or governmental request.
                </li>
              </ul>
            </div>
          </div>
          <div class="privacy-div-2">
            <h6 class="privacy-text-wrapper-2 h6-medium">Data Security</h6>
            <div class="privacy-p lg-regular">
              We implement a variety of security measures to protect personal
              data from unauthorized access, use, or disclosure. However, please
              note that no method of transmission over the Internet or
              electronic storage is completely secure, and we cannot guarantee
              absolute security.
            </div>
          </div>
          <div class="privacy-div-2">
            <h6 class="privacy-text-wrapper-2 h6-medium">
              Your Rights and Choices
            </h6>
            <div class="privacy-p lg-regular">
              As a user of {{ store.MediAssistConfigManager.DomainName }}, you
              have the following rights concerning your personal information:
              <ul>
                <li>
                  <span><b>Access :</b></span>
                  You can request access to the personal data we have collected
                  about you.
                </li>
                <li>
                  <span><b>Correction :</b></span>
                  You can request corrections to any inaccuracies in your
                  personal information.
                </li>
                <li>
                  <span><b>Deletion :</b></span>
                  You may request that we delete your personal information and
                  recorded conversations, subject to certain limitations.
                </li>
              </ul>
              To exercise any of these rights, please contact us at
              <a
                :href="`mailto:${store.MediAssistConfigManager.SupportEmail}`"
                >{{ store.MediAssistConfigManager.SupportEmail }}</a
              >.
            </div>
          </div>
          <div class="privacy-div-2">
            <h6 class="privacy-text-wrapper-2 h6-medium">Children’s Privacy</h6>
            <div class="privacy-p lg-regular">
              {{ store.MediAssistConfigManager.DomainName }} is not intended for
              use by children under the age of 13, and we do not knowingly
              collect personal information from children.<br />
              The doctor, as the user of
              {{ store.MediAssistConfigManager.DomainName }}, must obtain
              explicit consent from the guardian of children under the age of 13
              before providing their personal information into
              {{ store.MediAssistConfigManager.DomainName }}.<br />
              If we discover that we have collected personal data from a child
              under the age of 13 without the explicit consent of the guardian,
              we will take steps to delete that information as quickly as
              possible.
            </div>
          </div>
          <div class="privacy-div-2">
            <h6 class="privacy-text-wrapper-2 h6-medium">
              Changes to This Privacy Policy
            </h6>
            <div class="privacy-p lg-regular">
              We may update this Privacy Policy from time to time. Any changes
              will be reflected on this page with an updated "Last Updated"
              date. We encourage you to review this policy periodically to stay
              informed about how we are protecting your information.
            </div>
          </div>
          <div class="privacy-div-2">
            <h6 class="privacy-text-wrapper-2 h6-medium">Contact Us</h6>
            <div class="privacy-p lg-regular">
              If you have any questions, concerns, or requests regarding this
              Privacy Policy or the data we collect, please contact us at:
              <ul>
                <li>
                  <span><b>Email :</b></span> &nbsp;
                  <a
                    :href="`mailto:${store.MediAssistConfigManager.SupportEmail}`"
                    >{{ store.MediAssistConfigManager.SupportEmail }}</a
                  >
                </li>
              </ul>
              By using {{ store.MediAssistConfigManager.DomainName }}, you
              acknowledge that you have read and understand this Privacy Policy,
              and you agree to the collection and use of your information as
              described.
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { useMyStore } from "@/store/store.ts";

const store = useMyStore();
const privacyFrame = ref(null);
</script>

<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Karma:wght@400;500&display=swap");

.privacy-page {
  width: 100%;
  position: relative;
  background: linear-gradient(0deg, #050613 0%, #051b2f 100%);
}
.privacy-header {
  width: 100%;
  height: 230px;
}
.privacy-text {
  color: #ffffff;
  letter-spacing: 0;
  top: 125px;
  position: relative;
  margin: 0px;
  margin-inline: auto;
  width: 1277px;
}
.privacy-frame {
  align-items: flex-start;
  background-color: #f7fbff;
  display: flex;
  flex-direction: column;
  gap: 10px;
  padding: 60px 120px;
  position: relative;
}
.privacy-frame-wrapper {
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
.privacy-div {
  align-items: flex-start;
  display: flex;
  flex: 1;
  flex-direction: column;
  flex-grow: 1;
  gap: 25px;
  position: relative;
}
.privacy-text-wrapper {
  align-self: stretch;
  color: #1c1c1c;
  letter-spacing: 0;
  margin-top: -1px;
  margin-bottom: -1px;
  position: relative;
}
.privacy-div-2 {
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 10px;
  position: relative;
}
.privacy-text-wrapper-2 {
  align-self: stretch;
  color: #1c1c1c;
  letter-spacing: 0;
  margin-top: -1px;
  position: relative;
}

.privacy-p {
  align-self: stretch;
  color: #1c1c1c;
  letter-spacing: 0;
  position: relative;
}

.privacy-p a {
  color: #0066d4;
  text-decoration-line: underline;
  text-underline-offset: auto;
  text-underline-position: from-font;
  text-decoration-style: solid;
  text-decoration-skip-ink: none;
  text-decoration-thickness: auto;
}
</style>
