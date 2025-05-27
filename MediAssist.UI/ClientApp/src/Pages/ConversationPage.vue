<template>
    <div class="conversation-page">
        <div>
            <SideBar v-if="!isMobileScreen" />
        </div>
        <div class="header-section">
            <HamburgerMenu v-if="isMobileScreen" />
            <span>
                Home
                <svg width="6" height="10" viewBox="0 0 6 10" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path fill-rule="evenodd" clip-rule="evenodd" d="M0.674613 0.269107C0.884276 0.100628 1.19993 0.123391 1.37964 0.31995L5.37964 4.69495C5.54013 4.87049 5.54013 5.12952 5.37964 5.30507L1.37964 9.68007C1.19993 9.87663 0.884276 9.89939 0.674613 9.73091C0.46495 9.56243 0.44067 9.26651 0.620381 9.06995L4.34147 5.00001L0.620381 0.930067C0.44067 0.733508 0.46495 0.437586 0.674613 0.269107Z" fill="#707070" />
                </svg>
                Consultation
            </span>
            <ALERTMESSAGE :message="alertMessage"
                          :type="alertType"
                          :isVisible="isAlertVisible"
                          @close="closeAlert" />
            <div class="header-icons">
                <div class="conversation-actions">
                    <button class="clear-session"
                            :class="{ disabled: !isClearSessionButtonEnabled}"
                            @click="showClearConfirmation">
                        Clear
                    </button>
                    <button class="generate-results"
                            :class="{ disabled: showReportBtn || !GenerateTranscriptionReposrt}"                            
                            @click="handleGenerateResults">
                        Generate Results
                    </button>
                    <PatientDetailsDialog v-if="isReportValidationShow" />

                </div>
                <div user-menu>
                    <img :src="image"
                         alt="Doctor Icon"
                         class="doctor-icon"
                         @click="toggleMenu" />
                </div>
                <div class="user-profile">
                    <ProfileDropDown :menuOpen="menuOpen" />
                </div>
            </div>
        </div>
        <div class="maincontent-container">
            <div class="patient-info">
                <div class="frame-group">
                    <div class="patient-info-wrapper">
                        <h2 class="patient-info-title2">Patient Details</h2>
                    </div>
                    <div class="form-group" id="patient-info-form">
                        <div class="patient-id-container">
                            <div class="group-div">
                                <div class="patient-id-custom">
                                    <input type="text"
                                           name="patientId"
                                           id="patientId"
                                           v-model="patientId"
                                           class="input-field"
                                           placeholder="Patient Id eg:MRN123"
                                           maxlength="20"
                                           @input="validatePatientId"
                                           required />
                                    <span v-if="!isValidPatientId" class="error-message">Please enter a valid Patient Id</span>
                                </div>
                            </div>
                        </div>
                        <div class="name-container">
                            <div class="name-wrapper">
                                <div class="input-container">
                                    <input type="text"
                                           name="name"
                                           id="name"
                                           v-model="name"
                                           class="input-field"
                                           placeholder="Name"
                                           maxlength="30"
                                           @input="validateName"
                                           @keypress="preventNumbers" />
                                    <span v-if="!isValidName" class="error-message">Please enter a valid name</span>
                                </div>
                            </div>
                        </div>
                        <div class="gender-container">
                            <div class="gender-wrapper">
                                <div class="gender-select">
                                    <select name="gender"
                                            id="gender"
                                            v-model="gender"
                                            class="input-field gender-Opetion-text-color">
                                        <option value="" disabled hidden class="gender-placeholder">
                                            Gender
                                        </option>
                                        <option class="gender-Options gender-Opetion-text-color" value="Male">Male</option>
                                        <option class="gender-Options gender-Opetion-text-color" value="Female">Female</option>
                                        <option class="gender-Options gender-Opetion-text-color" value="Transgender">Transgender</option>
                                        <option class="gender-Options gender-Opetion-text-color" value="Non-binary">Non-binary</option>
                                        <option class="gender-Options gender-Opetion-text-color" value="Prefer Not to Say">Prefer Not to Say</option>
                                    </select>
                                    <span v-if="!isValidGender" class="error-message">Please select a gender</span>
                                </div>
                            </div>
                        </div>
                        <div class="age-container">
                            <div class="age-wrapper">
                                <div class="age-input">
                                  <input
                                  placeholder="Date of Birth"
                                  class="dob-picker"
                                  type="text"
                                  onfocus="(this.type='date')"
                                  v-model="dob"
                                  @keypress="onlyAllowNumbers"
                                  @blur="validateDob"
                                  @input="validateDob"
                                  :max="getCurrentDate()"
                                   />
                                  <span v-if="dobErrors.blankDob" class="error-message">
                                  Date of Birth cannot be left blank.
                                </span>
                                <span v-if="dobErrors.invalidDateFormat" class="error-message">
                                  Please enter a valid date.
                                </span>
                                <span v-if="dobErrors.futureDate" class="error-message">
                                Date of birth cannot be a future date.
                              </span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="main-content">
                <div class="conversations">
                    <div class="conversation-header">
                        <h2 class="h2-responsive-size">Your conversations</h2>
                        <div class="conversation-actions">
                            <LanguageDropdown v-if="!sessionStarted && !sessionStopped" @languageChange="handleLanguageChange" />
                            <button v-if="!sessionStarted"
                                    @click="handleStartSessionClick"
                                    :disabled="sessionStopped ||  AvailableHoursExceeded"
                                    :class="[sessionStopped ? 'disabled-button' : 'start-session']">
                                <img src="@/assets/play-button.png"
                                     alt="Play Button"
                                     class="play-button" />
                                Start Session
                            </button>
                            <button v-if="sessionStarted && !sessionStopped"
                                    @click="togglePauseResume"
                                    class="start-session"
                                    :disabled="showPopup">
                                <img v-if="!sessionPaused"
                                     src="@/assets/Pause.png"
                                     alt="Pause Button"
                                     class="pause-button" />
                                <img v-else
                                     src="@/assets/play-button.png"
                                     alt="Play Button"
                                     class="play-button" />
                                {{ sessionPaused ? "Resume Session" : "Pause Session" }}
                            </button>
                            <button v-if="sessionStarted && !sessionStopped"
                                    @click="showConfirmation"
                                    class="stop-button">
                                <img src="@/assets/stop.png" alt="Stop Button" class="stop" />
                            </button>
                        </div>
                    </div>
                    <div class="conversation-content">
                        <div class="conversation-audio">
                            <div class="wave-animation">
                                <img v-if="recording && !isAzureLoading"
                                     src="@/assets/waveform-animated.gif"
                                     alt="Recording Waveform" />
                                <img v-if="recording && !isAzureLoading"
                                     src="@/assets/waveform-animated.gif"
                                     alt="Recording Waveform" />
                                <img v-if="recording && !isAzureLoading"
                                     src="@/assets/waveform-animated.gif"
                                     alt="Recording Waveform" />
                                <img v-if="recording && !isAzureLoading"
                                     src="@/assets/waveform-animated.gif"
                                     alt="Recording Waveform" />
                                <img v-else
                                     src="@/assets/waveform-static.png"
                                     alt="Static Waveform"
                                     class="centered-image" />
                            </div>
                        </div>
                        <div class="conversation-text">
                            <span class="no-data-found" v-if="conversation.length === 0">
                                Real-time conversation will appear here once the session starts.
                            </span>
                            <ul class="conversation-text-with-flex">
                                <li v-for="(message, index) in conversation"
                                    :key="index"
                                    :class="{
                    'guest1-message': message.sender === 'Guest-1',
                    'guest2-message': message.sender === 'Guest-2',
                  }"
                                    class="message-bubble">
                                    <AnimatedMessage :text="message.text" />
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>

                <div class="right-side-content">
                    <div class="notes-section">
                        <div class="add-section-heading">
                            <h3 class="h3-responsive-size"
                                :class="{ active: activeSection === 'notes' }"
                                @click="toggleSection('notes')">
                                Add Notes
                            </h3>
                            <h3 class="h3-responsive-size"
                                :class="{ active: activeSection === 'prescription' }"
                                @click="toggleSection('prescription')">
                                Add Prescription
                            </h3>
                        </div>

                        <div v-if="activeSection === 'notes'">
                            <div class="notes-input-container">
                                <textarea v-model="newNote"
                                          placeholder="Write your notes here."
                                          @keyup.enter="addNote"
                                          class="notes-input"
                                          maxlength="200"></textarea>
                                <button class="add-note-btn" @click="addNote">
                                    <img src="@/assets/union.png"
                                         alt="Union Icon"
                                         class="union-icon" />
                                </button>
                            </div>
                            <div class="notes-list">
                                <div v-for="(note, index) in notes"
                                     :key="index"
                                     class="note-item">
                                    <input v-if="editingNoteIndex === index"
                                           v-model="notes[index]"
                                           @blur="finishEditingNote(index)"
                                           @keyup.enter="finishEditingNote(index)"
                                           class="note-text editable" />
                                    <span v-else class="note-text">{{ note }}</span>
                                    <button v-if="editingNoteIndex !== index" class="close-btn" @click="startEditingNote(index)">
                                        <img src="@/assets/icons/edit.svg"
                                             alt="Edit Icon"
                                             class="close-icon" />
                                    </button>
                                    <button class="close-btn" @click="removeNote(index)">
                                        <img src="@/assets/close-icon.png"
                                             alt="Close Icon"
                                             class="close-icon" />
                                    </button>
                                </div>
                            </div>
                        </div>

                        <div v-if="activeSection === 'prescription'">
                            <div class="notes-input-container">
                                <textarea v-model="newPrescription"
                                          placeholder="Write your prescription here."
                                          class="notes-input"
                                          maxlength="200"
                                          :readonly="!sessionStopped"
                                          :class="{ disabled: !sessionStopped }"
                                          @keyup.enter="addPrescription"></textarea>
                                <button class="add-note-btn" @click="addPrescription">
                                    <img src="@/assets/union.png"
                                         alt="Union Icon"
                                         class="union-icon" />
                                </button>
                            </div>
                            <div class="prescription-list">
                                <div v-for="(prescription, index) in prescriptions"
                                     :key="index"
                                     class="note-item">
                                    <input v-if="editingPrescriptionIndex === index"
                                           v-model="prescriptions[index]"
                                           @blur="finishEditingPrescription(index)"
                                           @keyup.enter="finishEditingPrescription(index)"
                                           class="note-text editable"
                                           ref="editPrescriptionInput" />
                                    <span v-else class="note-text"><AnimatedMessage :text="prescription" /></span>
                                    <button v-if="sessionStopped && editingPrescriptionIndex !== index" class="close-btn" @click="startEditingPrescription(index)">
                                        <img src="@/assets/icons/edit.svg"
                                             alt="Edit Icon"
                                             class="close-icon" />
                                    </button>
                                    <button v-if="sessionStopped && editingPrescriptionIndex !== index" class="close-btn" @click="removePrescription(index)">
                                        <img src="@/assets/close-icon.png"
                                             alt="Close Icon"
                                             class="close-icon" />
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div v-if="activeSection === 'prescription'" class="footer">
                            <p class="footer-text">
                                <img src="@/assets/disclaimer.png"
                                     alt="Disclaimer Icon"
                                     class="disclaimer-icon" />
                                     The prescriptions generated by AI are automatically captured from the conversation once the session starts and are based on the information provided during the consultation.
                            </p>
                        </div>
                    </div>
                    <div class="potential-diagnosis">
                      <div class="potential-diagnosis-toggle">
                        <h3>Potential Diagnosis</h3>
                        <div class="toggle-container">
                          <label class="switch">
                            <input type="checkbox" @change="toggleDiagnosis" :disabled="isTranscriptionStarted" :checked="showDiagnosis" />
                            <span class="slider round"></span>
                          </label>
                          <span class="toggle-text">{{ showDiagnosis ? "On" : "Off" }}</span>
                        </div>
                      </div>
                      <div v-if="diagnosisList">
                          <div>Based on the conversation, potential diagnoses are: </div>
                          <div v-for="(diagnosis, index) in diagnosisList" :key="index">
                              <br>&ensp;&bull; <AnimatedMessage :text="diagnosis" />
                          </div>
                      </div>
                      <span v-else class="no-data-found">No records available</span>
                      <div class="footer">
                          <p class="footer-text">
                              <img src="@/assets/disclaimer.png"
                                    alt="Disclaimer Icon"
                                    class="disclaimer-icon" />
                              This AI-powered doctor assistant provides general guidance based on input data but is not a substitute for professional medical advice, diagnosis, or treatment.
                          </p>
                      </div>
                    </div>
                </div>
            </div>
        </div>
        <DiagnosisPopup v-if="showDiagnosisPopup" @closePopup="showDiagnosisPopup = false" />
        <StopSessionAlert v-if="showPopup"
                          :isVisible="showPopup"
                          title="Confirm Stop Session"
                          message="Are you sure you want to stop the session?"
                          @confirm="confirmStop"
                          @cancel="cancelStop" />

        <ClearSessionAlert v-if="showClearPopup"
                           :isVisible="showClearPopup"
                           title="Confirm Clear Session"
                           message="Are you sure you want to clear this session? All transcription and prescription data will be lost."
                           confirmText="Yes"
                           cancelText="Cancel"
                           @confirm="confirmClearSession"
                           @cancel="cancelClearSession" />


        <GenerateReportAlert :isVisible="showGenerateReportAlert"
                             title="Confirmation Required"
                             message="Please review the changes. Confirming will generate the report based on the current diagnosis and prescription. Do you wish to proceed?"
                             confirmText="Confirm Action"
                             cancelText="Cancel"
                             @confirm="confirmationRequired"
                             @cancel="cancelAction" />

        <GenerateResultPreviewPopUp v-if="showGenerateResultPreviewPopUp"
                                    :reportData="reportData"
                                    @close="closeGenerateResultPreviewPopUp" />
        <LoadingSpinner :visible="isLoading"
                        :message="'Generating report, please wait...'" />

        <FeedBackForm :isVisible="showConfirmationPopup"
                      :title="FeedbackTitle"
                      :message="feedBack"
                      :cancelText="cancelText"
                      :confirmText="confirmText"
                      @confirm="Requiresconfirmation"
                      @cancel="cancelationAction" />

        <FeedbackPopup :isVisible="showFeedbackPopup"
                       :title="PopupTitle"
                       :confirmText="FeedbackMessage"
                       @confirm="confirmPopup"
                       @cancel="cancelPopup"
                       @setRating="setRating"
                       @update:selectedItems="handleSelectedCategories"
                       @text:otherCategoryText="handleCustomCategoryText" />

        <FeedbackSuccessPopup :isVisible="showSucessPopup"
                              :title="sucessTitle"
                              :message="sucessMessage"
                              @cancel="successCancel" />

        <FeedbackErrorPopup :isVisible="showErrorPopup"
                            :isError="showErrorPopup"
                            :title="erorTitle"
                            :message="errorMessage"
                            :tryAgain="tryAgainButton"
                            @confirm="tryAgain"
                            @cancel="successCancel" />
        <LoadingSpinner :visible="isAzureLoading"
                        :message="'Intializing the session, please wait...'" />
        <SessionEndWaring
        :showSessionWillEndSoon = "showSessionWillEndSoon"
          @close="CloseSessionWarning"
         v-if="showPlanExpired && !isAlertVisible"/>

        <SettingUpdateAlert v-if="showSettingsPopup"
                          :isVisible="showSettingsPopup"
                          title="Profile and Settings Incomplete"
                          @confirm="confirmSettingsUpdated"
                          @cancel="cancelSettingsPopup" />

        <PatientConsentConfirmation v-if="showPatientConsentConfirmation"
                                    :isVisible="showPatientConsentConfirmation"
                                    title="Patient Consent Confirmation"
                                    message="Please confirm that you have obtained the patient’s consent to use this application and store their medical data."
                                    confirmText="Yes"
                                    cancelText="Cancel"
                                    @confirm="confirmPatientConsent"
                                    @cancel="ClosePatientConsentConfirmation" />                  
    </div>
</template>

<script setup>
import Icon from "@/assets/doctor-icon.png";
import ALERTMESSAGE from '@/components/alert/AlertMessage.vue';
import { default as ClearSessionAlert, default as FeedBackForm, default as GenerateReportAlert, default as PatientConsentConfirmation } from "@/components/alert/ConfirmationAlert.vue";
import FeedbackPopup from "@/components/alert/FeedbackPopup.vue";
import { default as FeedbackErrorPopup, default as FeedbackSuccessPopup } from "@/components/alert/FeedbackSuccessErrorPopup.vue";
import StopSessionAlert from "@/components/alert/StopSessionAlert.vue";
import AnimatedMessage from '@/components/AnimatedMessage.vue';
import LanguageDropdown from '@/components/languageSelector.vue';
import LoadingSpinner from '@/components/LoadingSpinner.vue';
import PatientDetailsDialog from "@/components/PatientDetailsDialog.vue";
import ProfileDropDown from "@/components/ProfileDropDown.vue";
import SideBar from "@/components/SideBar.vue";
import SessionEndWaring from "@/components/TosterMessges/SessionEndWaring.vue";
import GenerateResultPreviewPopUp from '@/Pages/modals/GenerateResultPreviewPopUp.vue';
import { useMyStore } from '@/store/store.ts';
import axiosInstance from '../Services/Interceptors/axios.js';
import * as SpeechSDK from "microsoft-cognitiveservices-speech-sdk";
import "primeicons/primeicons.css";
import { io } from "socket.io-client";
import { computed, nextTick, onBeforeMount, onBeforeUnmount, onMounted, onUnmounted, reactive, ref, watch } from "vue";
import { useRouter } from 'vue-router';
import { AZURE_CONFIG, PYTHON_API_URL } from "../Config.js";
import SettingUpdateAlert from "@/components/alert/SettingUpdateAlert.vue";
import DiagnosisPopup from "@/components/alert/PotentialDiagnosisPopUp.vue";

const conversation = ref([]);
const diagnosisList = ref(null);
const recording = ref(false);
const diagnosisSocket = ref(null);
const activeSection = ref('prescription');
const newNote = ref('');
const notes = ref([]);
const newPrescription = ref('');
const prescriptions = ref([]);
const editingNoteIndex = ref(-1);
const editingPrescriptionIndex = ref(-1);
const cumulativeTranscript = ref('');
const sessionStarted = ref(false);
const sessionPaused = ref(false);
const sessionStopped = ref(false);
const showPopup = ref(false);
const showClearPopup = ref(false);
const menuOpen = ref(false);
const patientId = ref("");
const name = ref("");
const gender = ref("");
const age = ref("");
const showReportBtn = ref(true);
const isValidName = ref(true);
const isValidPatientId = ref(true);
const isValidGender = ref(true);
const isReportValidationShow = ref(false);
const showGenerateReportAlert = ref(false);
const showGenerateResultPreviewPopUp = ref(false);
const alertMessage = ref('');
const alertType = ref('info');
const isAlertVisible = ref(false);
const isResultGenerated = ref(false);
const isLoading= ref(false);
const signature = ref("");
const logo = ref("");
const showPatientConsentConfirmation = ref(false);
const patientConsent = ref(false);
const reportData = reactive({
  hospitalInfo: {
    hospitalName: '',
    hospitalAddress: '',
    doctorName: '',
    doctorSpecialty: '',
    doctorTitle: '',
    doctorSignature: '',
    hospitalLogo: ''
  },
  patientInfo: {
    mrn: '',
    name: '',
    age: '',
    gender: '',
    medinotexPatientId : ''
  },
  reports: {
    summary: {
      cheifComplaint: '',
      medicalHistory: '',
      observation: '',
      treatmentPlan: '',
      prescriptions: ''
    },
    prescriptions: {
      Laboratory_Tests: [],
      Medications: {
        Name: '',
        Dosage: '',
        Frequency: '',
        Duration: ''
      }
    }
  },
});
const image = ref("");
const userId = ref("");
const showConfirmationPopup = ref(false);
const FeedbackTitle = ref("We Value Your Feedback!");
const FeedbackMessage = ref("Submit Feedback");
const cancelText = ref("Maybe Later");
const confirmText = ref("Submit");
const showFeedbackPopup = ref(false);
const feedBack = ref( "Please provide your valuable insights to help us improve your experience.");
const PopupTitle = ref("Share Your Feedback");
const sucessTitle = ref("Submission Successful");
const sucessMessage = ref("Thank you for your feedback! It has been successfully submitted.");
const showSucessPopup = ref(false);
const showErrorPopup = ref(false);
const erorTitle = ref("Submission Failed");
const errorMessage = ref("There was a problem submitting your feedback. Please try again later.");
const tryAgainButton = ref("Try Again");
const rating = ref(0);
const selectedCategories = ref([]);
const customCategoryText = ref("");
const recognizer = ref(null);
const selectedLanguage = ref('en-US');
const isAzureLoading = ref(false);
const userSessionId  = ref(0);
let updateSessionIntervalId;
const store = useMyStore();
const AvailableHoursExceeded = ref(false);
const GenerateTranscriptionReposrt = ref(true)
let NumberOfTranscriptions = 0;
let conversationSpeechConfig = null;
let audioConfig = null;
const showPlanExpired = ref(false)
let showSessionWillEndSoon = false;
let showSessionWillEndSoonPopUp = true;
let totalCost = 0.0;
let totalToken = 0;
const router = useRouter();
const showSettingsPopup = ref(false);
const showDiagnosis = ref(true);
const isTranscriptionStarted = ref(false);
const showDiagnosisPopup = ref(false);
const medinotexPatientId = ref("");
const dob = ref("");

const updateUserCounters = async () => {
  try {
    const response = await axiosInstance.post(`/api/manage/update-counter?userId=${userId.value}`);
    if (response.data.success) {
      if(response.data.count == 3){
        showConfirmationPopup.value = true;
      }
      else{
        showConfirmationPopup.value = false;
      }
    }
  } catch (error) {
    console.error('Error updating counters');
  }
};

const Requiresconfirmation = () => {
  showFeedbackPopup.value = true;
  showConfirmationPopup.value = false;
};

const cancelationAction = () => {
  showConfirmationPopup.value = false;
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
    if (response.data.success) {
      showSucessPopup.value = true;
      showFeedbackPopup.value = false;
    } else {
      showErrorPopup.value = true;
    }
  } catch (error) {
    console.error("Error during feedback submission");
  }
};

const cancelPopup = () => {
  showFeedbackPopup.value = false;
};

const setRating = (star) => {
  rating.value = star;
};

const handleSelectedCategories = (categories) => {
  selectedCategories.value = categories.map(category => category.id);
};

const handleCustomCategoryText = (text) => {
  customCategoryText.value = text;
}

const successCancel = () => {
  showSucessPopup.value = false;
  showErrorPopup.value = false;
};
const CloseSessionWarning = () => {
  showPlanExpired.value = false;

};
const tryAgain = () => {
  showSucessPopup.value = false;
  showErrorPopup.value = false;
};
const handleLanguageChange = (languageCode) => {
  selectedLanguage.value =languageCode;
};

const showAlert = (message, type = 'info') => {
  alertMessage.value = message;
  alertType.value = type;
  isAlertVisible.value = true;
  const isResultGenerated = ref(false);

watch(showGenerateResultPreviewPopUp, (newValue) => {
  if (!newValue) {
    isResultGenerated.value = false;
  }
});

  setTimeout(() => {
    closeAlert();
  }, 5000);
};

const closeAlert = () => {
  isAlertVisible.value = false;
};

const dobErrors = ref({
  invalidDateFormat: false,
  invalidAge: false,
  futureDate: false,
  blankDob: false,
});

const validatePatientId = () => {
  const regex = /^[A-Z0-9]{3,}$/;
  patientId.value = patientId.value.replace(/[^a-zA-Z0-9]/g, '').toUpperCase();
  isValidPatientId.value = regex.test(patientId.value);
};

const validateGender = () => {
  isValidGender.value = gender.value !== '';
};

const validateDob = () => {
  const today = new Date();
  const inputValue = dob.value;
  const selectedDate = new Date(inputValue);

  dobErrors.value = {
    invalidDateFormat: false,
    invalidAge: false,
    futureDate: false,
    blankDob: false,
  };

  if (!inputValue) {
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
  age.value = calculateAge(dob.value);
};

const getCurrentDate = () => {
  const today = new Date();
  const year = today.getFullYear();
  const month = String(today.getMonth() + 1).padStart(2, "0");
  const day = String(today.getDate()).padStart(2, "0");
  return `${year}-${month}-${day}`;
};

const calculateAge = (dob) => {
  const birthDate = new Date(dob);
  const today = new Date();

  if (isNaN(birthDate.getTime()) || birthDate > today) {
    return null; // Invalid or future date
  }
  let years = today.getFullYear() - birthDate.getFullYear();
  let months = today.getMonth() - birthDate.getMonth();
  let days = today.getDate() - birthDate.getDate();
  if (days < 0) {
    months--;
  }
  if (months < 0) {
    years--;
    months += 12;
  }
  if (years < 1) {
    return `${months} month`;
  }
  return `${years}`;
};

const validateName = () => {
  const nameRegex = /^[a-zA-Z][a-zA-Z\s']*$/;
  if (name.value.trim() === '' || name.value.length > 30 || !nameRegex.test(name.value)) {
    isValidName.value = false;
  } else {
    isValidName.value = true;
  }
};

const preventNumbers = (event) => {
  if (!/[a-zA-Z\s']/.test(event.key)) {
    event.preventDefault();
  }
};

const onlyAllowNumbers = (event) => {
  const charCode = event.which ? event.which : event.keyCode;
  if (charCode > 31 && (charCode < 48 || charCode > 57)) {
    event.preventDefault();
  }
};
watch(age, validateDob);
watch(patientId, validatePatientId);
watch(gender, validateGender);

const handleGenerateResults = async () => {
  if (showReportBtn.value && GenerateTranscriptionReposrt.value) {
    isReportValidationShow.value = true;
    setTimeout(() => {
      isReportValidationShow.value = false;
    }, 5000);
  }
  else if(showReportBtn.value && !GenerateTranscriptionReposrt.value){
    showPlanExpired.value = true;
    setTimeout(() => {
      showPlanExpired.value = false;
    }, 10000);
  } else if(!showReportBtn.value && GenerateTranscriptionReposrt.value){
    showGenerateReportAlert.value = true;
  }
};
const validatePatientInfoFields = () => {
  showReportBtn.value = !(
    patientId.value &&
    name.value &&
    gender.value &&
    age.value &&
    sessionStopped.value
  );
};
watch(name, () => {
  validateName();
  validatePatientInfoFields();
});
watch([patientId, gender, age, sessionStopped], validatePatientInfoFields);

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

const initializeDiagnosisSocket = () => {
  diagnosisSocket.value = io(PYTHON_API_URL);

  diagnosisSocket.value.on('suggestion_diagnosis', (data) => {
    if(!sessionStopped.value){
      diagnosisList.value = data;
    }
  });

  diagnosisSocket.value.on('suggestion_medicines', (data) => {
    prescriptions.value = data;
  });

  diagnosisSocket.value.on('llm_costs', (data) => {
    console.log("tolalCost:",data.total.Cost);
    console.log("totalToken:",data.total.token);
    totalCost = totalCost + data.total.Cost;
    totalToken = totalToken + data.total.token;
  });
};

const showConfirmation = () => {
  showPopup.value = true;
  recording.value = false;
  stopTranscription();
};

const confirmStop = () => {
  sessionStopped.value = true;
  sessionPaused.value = false;
  sessionStarted.value = false;
  recording.value = false;
  showPopup.value = false; 
  stopTranscription();
  StopSession();
  diagnosisSocket.value.emit("disconnectSocket");
  diagnosisSocket.value.disconnect();
  showAlert('Session ended. Please make any necessary adjustments to the prescription or notes before generating the report.', 'info');
};

const StartOrResumeSession = async () => {

  try {
    const response = await axiosInstance.post('api/Session/startusersession', {
        userid: userId.value,
        sessionId : userSessionId.value,
        TotalToken : totalToken,
        TotalCost : totalCost,
        isPotentialDiagnosisOn: showDiagnosis.value
    });
    const data = response.data;
      userSessionId.value = data.sessionId;

    if(sessionStarted.value && !sessionPaused.value && !sessionStopped.value){
        startSessionUpdation()
    }else{
      stopSessionUpdation();
    }
    if(data.sessionExpired){
        confirmStop();
      }
  } catch (error) {
    return false;
  }
};

const StopSession = async () => {
  try {
     await axiosInstance.post('api/Session/stopusersession', {
        userid: userId.value,
        sessionId : userSessionId.value,
        TotalToken : totalToken,
        TotalCost : totalCost
    });
    clearInterval(updateSessionIntervalId);
  } catch (error) {
    return false;
  }
};
const stopSessionUpdation = () => {
  if (updateSessionIntervalId) {
    clearInterval(updateSessionIntervalId);
    updateSessionIntervalId = null;
  }
};
const startSessionUpdation = () => {
  updateSessionIntervalId = setInterval(UpdateSession, 12000);
};

const UpdateSession = async () => {
  try {
    if(sessionStarted.value && !sessionPaused.value){      
    const response =  await axiosInstance.post('api/Session/updateusersession', {
        userid: userId.value,
        sessionId : userSessionId.value,
        TotalToken : totalToken,
        TotalCost : totalCost
    });
      const data = response.data;
      if(data.sessionExpired){
        confirmStop();
        clearInterval(updateSessionIntervalId);
      }
      if (data.remainingTime) {
    const timeParts = data.remainingTime.split(':'); // Split into ["HH", "mm", "ss"]
    const hours = parseInt(timeParts[0], 10);
    const minutes = parseInt(timeParts[1], 10);
    const seconds = parseInt(timeParts[2], 10);

    // Convert to total seconds
    const totalSeconds = hours * 3600 + minutes * 60 + seconds;
    if (totalSeconds < 120 ) {
      if(showSessionWillEndSoonPopUp){
        showPlanExpired.value = true;
      showSessionWillEndSoon = true;
      showSessionWillEndSoonPopUp = false
      }else{
        showSessionWillEndSoon = false;
      }
      
    }
}
    }
  } catch (error) {
    return false;
  }
};

const ReportGenerated = async () => {
  try {
     await axiosInstance.post('api/Session/reportgenerated', {
        userid: userId.value,
        sessionId : userSessionId.value,
        TotalToken : totalToken,
        TotalCost : totalCost
    });
    NumberOfTranscriptions = NumberOfTranscriptions - 1 ;
    } catch (error) {
    return false;
  }
};

const cancelStop = () => {
  showPopup.value = false;
  if (sessionStarted.value && !sessionStopped.value) {
    sessionPaused.value = true;
    recording.value = false;
  }
};


const confirmationRequired = async () => {
  showGenerateReportAlert.value = false;
  isLoading.value = true;
  await generateResults();
  await ReportGenerated();
  if(NumberOfTranscriptions == 0 ){
    GenerateTranscriptionReposrt.value = false
  }
  setTimeout(() => {
    isLoading.value = false;
    showGenerateResultPreviewPopUp.value = true;
  }, 0);
};

const cancelAction = () => {
  showGenerateReportAlert.value = false;
}

const initializeSpeechConfig = () => {
  const speechKey = AZURE_CONFIG.speechKey;
  const serviceRegion = AZURE_CONFIG.serviceRegion;

  audioConfig = SpeechSDK.AudioConfig.fromDefaultMicrophoneInput();

  conversationSpeechConfig = SpeechSDK.SpeechConfig.fromSubscription(
    speechKey,
    serviceRegion
  );

  conversationSpeechConfig.speechRecognitionLanguage = selectedLanguage.value;

  const conversationTranscriber = new SpeechSDK.ConversationTranscriber(
    conversationSpeechConfig,
    audioConfig
  );

  return {
    conversationTranscriber,
    audioConfig
  };
};

const startTranscription = async () => {
  try {
    isTranscriptionStarted.value = true;
    isAzureLoading.value = true;
    recording.value = false;

    const { conversationTranscriber } = initializeSpeechConfig();

    conversationTranscriber.transcribed = (s, e) => {
      const text = e.result.text?.trim();
      if (text) {
        const speakerId = e.result.speakerId === "Guest-1" ? "Guest-1" : "Guest-2";

        conversation.value.push({
          sender: speakerId,
          text: text,
        });

        const previousTranscript = cumulativeTranscript.value.trim();
        cumulativeTranscript.value += `${text}`;

        const transcriptionPayload = {
          transcription: cumulativeTranscript.value,
          notes: notes.value,
          showDiagnosis: showDiagnosis.value,
        };

        if (diagnosisSocket.value) {
          if (cumulativeTranscript.value?.trim() !== previousTranscript.trim()) {
            diagnosisSocket.value.emit("transcription", transcriptionPayload);
          }
        }
      }
    };

    const handleError = () => {
      isAzureLoading.value = false;
    };

    conversationTranscriber.canceled = handleError('Conversation Transcription');

    conversationTranscriber.startTranscribingAsync(
      () => {
        recording.value = true;
        isAzureLoading.value = false;
      },
      handleError('Conversation Transcription Start')
    );

    recognizer.value = { conversationTranscriber };

  } catch (err) {
    recording.value = false;
    isAzureLoading.value = false;
  }
};

const stopTranscription = async () => {
  try {
    if (recognizer.value && recognizer.value.conversationTranscriber) {
      recognizer.value.conversationTranscriber.stopTranscribingAsync(
        () => {
          recording.value = false;
          isAzureLoading.value = false;
        },
          (err) => {
              throw err;
        }
      );
    }
  } catch (err) {
    recording.value = false;
    isAzureLoading.value = false;
  }
};

const handleStartSessionClick = () => {
  showPatientConsentConfirmation.value = true
}

const confirmPatientConsent = () => {
  startSession()
  showPatientConsentConfirmation.value = false
  patientConsent.value = true
}

const ClosePatientConsentConfirmation = () => {
  showPatientConsentConfirmation.value = false;
  patientConsent.value = false
};

const startSession = async () => {
  
  showDiagnosisPopup.value = false;
  const response = await axiosInstance.get(`/api/settings/check-settings-updated?userId=${userId.value}`);

  if (!response.data.isSettingsUpdated) {
    showSettingsAlertPopup();
  } else {
    if (sessionStarted.value) return;

    sessionStarted.value = true;
    sessionPaused.value = false;
    sessionStopped.value = false;
    recording.value = true;
    await startTranscription();
    StartOrResumeSession();
  }
};

const showSettingsAlertPopup = () => {
  showSettingsPopup.value = true;
};

const confirmSettingsUpdated = () => {
  router.push("/settings");
};

const cancelSettingsPopup = () => {
  showSettingsPopup.value = false;
}

const togglePauseResume = () => {
  if (recognizer.value && recognizer.value.conversationTranscriber) {
    const { conversationTranscriber } = recognizer.value;

    if (!sessionPaused.value) {
      sessionPaused.value = true;
      conversationTranscriber.stopTranscribingAsync(
        () => {
          recording.value = false;
        },
        (err) => {
          throw err;
        }
      );
    } else {
      sessionPaused.value = false;
      conversationTranscriber.startTranscribingAsync(
        () => {
          recording.value = true;
        },
        (err) => {
          throw err;
        }
      );
    }
    StartOrResumeSession();
  }
};

const toggleSection = (section) => {
  activeSection.value = section;
};

const addNote = () => {
  if (newNote.value.trim()) {
    notes.value.push(newNote.value.trim());
    newNote.value = '';
  }
};

const removeNote = (index) => {
  if (index >= 0 && index < notes.value.length) {
    notes.value.splice(index, 1);
  }
};

const startEditingNote = (index) => {
  editingNoteIndex.value = index;
  nextTick(() => {
    if (editingNoteIndex.value === index) {
      const inputElement = document.querySelector(`.note-item:nth-child(${index + 1}) input.note-text.editable`);
      if (inputElement) {
        inputElement.focus();
      }
    }
  });
};

const finishEditingNote = (index) => {
  if (index >= 0 && index < notes.value.length) {
    const noteText = notes.value[index];
    if (noteText === undefined || noteText.trim() === '') {
      removeNote(index);
    }
  }
  editingNoteIndex.value = -1;
};

const addPrescription = () => {
  if (newPrescription.value.trim()) {
    prescriptions.value.push(newPrescription.value.trim());
    newPrescription.value = '';
  }
};

const removePrescription = (index) => {
  if (index >= 0 && index < prescriptions.value.length) {
    prescriptions.value.splice(index, 1);
  }
};

const startEditingPrescription = (index) => {
  editingPrescriptionIndex.value = index;
  nextTick(() => {
    if (editingPrescriptionIndex.value === index) {
      const inputElement = document.querySelector(`.note-item:nth-child(${index + 1}) input.note-text.editable`);
      if (inputElement) {
        inputElement.focus();
      }
    }
  });
};

const finishEditingPrescription = (index) => {
  if (index >= 0 && index < prescriptions.value.length) {
    const prescriptionText = prescriptions.value[index];
    if (prescriptionText === undefined || prescriptionText.trim() === '') {
      removePrescription(index);
    }
  }
  editingPrescriptionIndex.value = -1;
};

const generateResults = async () => {
  try {
   await  mapDoctorDetails(userId.value);
    mapPatientDetails();

    const response = await axiosInstance.post(`${PYTHON_API_URL}/request_report`, {
      transcription: cumulativeTranscript.value,
      notes: notes.value,
      medlab: prescriptions.value,
    });

    if (response && response.data.Summary_Report) {
      mapReportDetails(response.data.Summary_Report);
      if (response.data.Treatment_Plan) {
        mapPrescriptionDetails(response.data.Treatment_Plan);
      }
      isResultGenerated.value = true;
      showGenerateResultPreviewPopUp.value = true;
    }
  } catch (error) {
    isResultGenerated.value = false;
    showGenerateResultPreviewPopUp.value = false;
  }

};

const mapPatientDetails = () => {
  reportData.patientInfo = {
    mrn: patientId.value,
    name: name.value,
    gender: gender.value,
    age: age.value,
    medinotexPatientId:medinotexPatientId.value
  };
};

const mapDoctorDetails = async (userId) => {
  try{    
    const response = await axiosInstance.get(`/api/settings/get-report-details?UserId=${userId}`);
    if(response.data.success){

          const clinicId = response.data.clinicId ;
          const payload = {
                MrnNumber: patientId.value,
                PatientName: name.value,
                Gender: gender.value,
                Age: age.value,
                DoB: dob.value,
                UserId: userId,
                ClinicId :clinicId,
                PatientConsent : patientConsent.value
          };

      var insertResponse =  await  axiosInstance.post("/api/patient/insert-update-patient",payload);

      if(insertResponse.data.success){
        medinotexPatientId.value = insertResponse.data.patientId
      } 
         

      let signType = "png";
      if (response.data.doctorSignature != null) {
        if (response.data.doctorSignature.startsWith("/9j")) {
          signType = "jpeg";
        }
        signature.value = `data:image/${signType};base64,${response.data.doctorSignature}`;
      }

      let logoType = "png";
      if (response.data.hospitalLogo != null) {
        if (response.data.hospitalLogo.startsWith("/9j")) {
          logoType = "jpeg";
        }
        logo.value = `data:image/${logoType};base64,${response.data.hospitalLogo}`;
      }

      reportData.hospitalInfo = {
        hospitalName: response.data.hospitalName,
        hospitalAddress: response.data.hospitalAddress,
        doctorName: response.data.doctorName,
        doctorSpecialty: response.data.doctorSpecialization,
        doctorTitle: response.data.doctorTitle,
        doctorSignature: signature.value,
        hospitalLogo: logo.value
      };
    }

  }catch (err) {
    console.error("An error occurred while fetching user data.");
  }
};

const mapReportDetails = (summaryReport) => {
  reportData.reports.summary = {
    cheifComplaint: summaryReport.Chief_Complaint || '',
    medicalHistory: summaryReport.Medical_History || '',
    observation: summaryReport.Observations || '',
    treatmentPlan: summaryReport.Treatment_Plan || ''
  };
};

const mapPrescriptionDetails = (prescriptionReport) => {
  reportData.reports.prescriptions = {
    Laboratory_Tests: prescriptionReport.Laboratory_Tests,
    Medications: prescriptionReport.Medications,
  };
};

const closeGenerateResultPreviewPopUp = () => {
  showGenerateResultPreviewPopUp.value = false;
  updateUserCounters();
};

const confirmClearSession = () => {
  try{
    location.reload();
  }catch (error) {
    showAlert("Unable to clear session at this time. Please try again.", "error");
    showClearPopup.value = false;
  }
};

const cancelClearSession = () => {
  showClearPopup.value = false;
};

    const showClearConfirmation = async () => {
  try{
    if (isClearSessionButtonEnabled.value) {
      showClearPopup.value = true;
      recording.value = false;
     await stopTranscription();
    }
  }catch (error) {
    showAlert("Clear action could not be completed. Please refresh the page and try again.", "error");
    showClearPopup.value = false;
  }
};

const isClearSessionButtonEnabled = computed(() => {
  return patientId.value.trim() !== "" ||
         name.value.trim() !== "" ||
         gender.value.trim() !== "" ||
         age.value.trim() !== "" ||
         sessionStopped.value ||
         notes.value.length > 0 ||
         prescriptions.value.length > 0;
});


function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
  return null;
}
const updateScreenWidth = () => {
  screenWidth.value = window.innerWidth;
};
const screenWidth = ref(window.innerWidth);
const isMobileScreen = computed(() => screenWidth.value < 420);
// Lifecycle hooks

onBeforeMount(async () => {
  let userId = getCookie("userId");
  await store.fetchUserActivityMetrics(userId);
  if(store.UserActivityMetrics.UserSessionsCount < 3){
    showDiagnosisPopup.value = true;
  }
  AvailableHoursExceeded.value = store.UserActivityMetrics.AvailableHours <= 0 ;
  NumberOfTranscriptions = store.UserActivityMetrics.Transcriptions;
  GenerateTranscriptionReposrt.value = store.UserActivityMetrics.Transcriptions > 0;
  if(AvailableHoursExceeded.value || !GenerateTranscriptionReposrt.value ){
    router.replace("/pricing");
  }
 })

onMounted(() => {
  initializeDiagnosisSocket();
  const imageValue = localStorage.getItem("image");
  userId.value = getCookie("userId");

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
  document.addEventListener("click", handleClickOutside);
  window.addEventListener('resize', updateScreenWidth);
});

onUnmounted(() => {
  document.removeEventListener("click", handleClickOutside);
  window.removeEventListener('resize', updateScreenWidth);
});

onBeforeUnmount(() => {
  stopTranscription();
  if (diagnosisSocket.value) {
    diagnosisSocket.value.disconnect();
  }
});

const toggleDiagnosis = () => {
  showDiagnosisPopup.value = false;
  if (!isTranscriptionStarted.value) {
    showDiagnosis.value = !showDiagnosis.value;
  }
};
</script>

<style scoped>

    .error-message{
      color: red;
      font-size: 12px;

    }
    .generate-results.disabled {
      cursor: not-allowed;
      background-color: #A9A9A9;
      border: 1px solid #A9A9A9;
      color: #ffffff;
      padding: 10px 20px;
      border-radius: 5px;
    }
    .generate-results {
      background-color: #2eb65c;
      border: 1px solid #2eb65c;
      color: #ffffff;
      padding: 10px 20px;
      border-radius: 5px;
      cursor: pointer;
    }
  
    .clear-session {
      background-color: red;
      border: 1px solid red;
      color: #ffffff;
      padding: 10px 20px;
      border-radius: 5px;
      cursor: pointer;
    }

    .clear-session.disabled {
      cursor: not-allowed;
      background-color: #A9A9A9;
      border: 1px solid #A9A9A9;
      color: #ffffff;
      padding: 10px 20px;
      border-radius: 5px;
    }

    .start-session:hover {
      background-color: #005bb5;
    }

    .stop-button {
      background-color: #f74949;
      color: white;
      border: none;
      border-radius: 8px;
      font-size: 16px;
      width: 40px;
      height: 40px;
      display: flex;
      padding: 11px 10px;
      align-items: center;
      gap: 10px;
    }
    .stop-button:hover {
      transform: scale(1.05);
    }

    .disabled-button {
      background-color: grey;
      color: white;
      padding: 10px;
      border: none;
      border-radius: 5px;
      font-size: 16px;
      margin-right: 10px;
      display: flex;
      align-items: center;
      gap: 10px;
      align-self: stretch;
    }

    .conversation-page {
      display: flex;
      flex-direction: column;
      background-color: #f5f7fa;
      height: 100vh;
      overflow-y: auto;
      position: relative;
    }

    .main-content {
      display: flex;
      padding: 18px;
      background-color: #f3f9ff;
    }

    .header-section {
      box-sizing: border-box;
      display: flex;
      justify-content: space-between;
      align-items: center;
      background-color: white;
      padding: 10px;
      border-bottom: 1px solid #ddd;
      width: calc(100% - 80px);
      margin-left: 80px;
      position: sticky;
      top: 0;
      z-index: 1;
    }
    .header-section span {
      font-size: 14px;
      font-weight: 400;
      line-height: 21px;
      text-align: left;
      text-underline-position: from-font;
      text-decoration-skip-ink: none;
      color: #1c1c1c;
      padding: 10px;
    }

    .header-icons {
      display: flex;
      align-items: center;
      gap: 20px;
    }

    .header-icons i {
      font-size: 20px;
      color: #007bff;
    }

    .doctor-icon {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      object-fit: cover;
      cursor: pointer;
    }

    .conversations {
      width: 673px;
      background: white;
      border-radius: 10px;
      padding: 40px;
      margin-right: 20px;
      display: flex;
      flex-direction: column;
      box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
      overflow: hidden;
    }

    .conversation-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 20px;
      flex-wrap: wrap;
    }

    .conversation-header h2 {
      margin: 0;
    }
    .conversation-actions {
      display: flex;
      gap: 10px;
      flex-wrap: wrap;
    }

    .no-data-found {
      align-items: center;
      color: #dee0e2;
      font-weight: 100;
      font-size: 20px;
    }

    .start-session {
        color: white;
        padding: 10px;
        border: none;
        border-radius: 5px;
        font-size: 16px;
        margin-right: 10px;
        display: flex;
        align-items: center;
        gap: 10px;
        background-color: #007bff;
        cursor: pointer;
    }


    .conversation-content {
        display: flex;
        flex-direction: column;
        gap: 20px;
    }

    .conversation-text {
      height: 703px;
      border-radius: 6px 0px 0px 0px;
      background: white;
      padding: 20px;
      box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
      overflow-y: auto;
    }
    .conversation-text-with-flex {
      display: flex;
      flex-direction: column-reverse;
      width:100%
    }
    .conversation-audio {
      height: auto;
      max-height: 100px;
      background: linear-gradient(
        180deg,
        rgba(244, 249, 255, 0.62) 0%,
        #c2dfff 50%,
        rgba(205, 229, 255, 0.31) 100%
      );
      padding: 10px;
      display: flex;
      justify-content: center;
      align-items: center;
      box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
      border-radius: 6px 0px 0px 0px;
    }

    .wave-animation {
      display: flex;
      align-items: center;
      width: 100%;
    }

    .wave-animation img {
      width: 100%;
      height: auto;
      object-fit: fill;
      border-radius: 6px;
      max-height: 120px;
    }

    .centered-image {
      width: 598px;
      height: 110px;
      border-radius: 6px 0px 0px 0px;
      display: block;
      margin: auto;
    }

    .conversation-text ul {
      list-style-type: none;
      padding: 0;
      margin: 0;
    }

    .message-bubble {
      max-width: 70%;
      padding: 10px 15px;
      margin: 10px 0;
      border-radius: 15px;
      line-height: 1.5;
      font-size: 16px;
      display: block;
      clear: both; /* Ensures each message starts on a new line */
      width: fit-content;
    }

    .guest1-message {
      background-color: #E6F2FF;
      color: #000;
      float: left;
      margin-left: 10px;
      border-radius: 8px 8px 8px 0px;
    }

    .guest2-message {
      background-color: #F5F5F5;
      color: #000;
      margin-right: 10px;
      border-radius: 8px 0px 8px 8px;
      float: right;
      align-self: flex-end;
    }

    .message-text {
      white-space: pre-wrap;
    }

    .icon-wrapper {
      width: 30px;
      height: 30px;
      margin-right: 10px;
    }

    .icon {
      width: 100%;
      height: 100%;
      border-radius: 50%;
      object-fit: cover;
    }

    .right-side-content {
      display: flex;
      flex-direction: column;
      gap: 20px;
      flex: 1;
    }

    .notes-section {
      height: 370px;
      display: grid;
      grid-template-rows: 18% 71% 11%;
      background: white;
      padding: 20px;
      border-radius: 10px;
      box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }

    .potential-diagnosis {
      height: 549px;
      display: grid;
      grid-template-rows: 10% 80% 10%;
      background: white;
      padding: 20px;
      border-radius: 10px;
      box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }

    .add-section-heading {
      display: flex;
      gap: 30px;
      align-items: center;
      cursor: pointer;
      font-weight: normal;
      padding: 10px;
    }

    .add-section-heading h3:hover {
      transform: scale(1.05);
    }

    .add-section-heading h3.active {
      color: #0066d4;
      font-weight: bold;
    }

    .notes-section h3 {
      margin-bottom: 10px;
    }

    .notes-input-container {
      display: flex;
      align-items: center;
      gap: 5px;
      box-sizing: border-box;
      border: 1px solid #ddd;
      border-radius: 5px;
      padding: 5px;
      margin-bottom: 15px;
    }

    .note-text.editable {
        background-color: #fff;
        border: 1px solid #007bff;
        border-radius: 3px;
        padding: 5px;
        width: 100%;
        box-sizing: border-box;
        outline: none;
    }

    .notes-input {
      padding: 10px;
      resize: none;
      overflow: hidden;
      font-size: 14px;
      border: none;
      outline: none;
      background-color: transparent;
      flex-grow: 1;
    }

    .add-note-btn {
      width: 30.93px;
      height: 30.93px;
      background-color: #fff;
      border: 1px solid #007aff;
      border-radius: 5px;
      color: #007aff;
      font-size: 18px;
      cursor: pointer;
      display: flex;
      justify-content: center;
      align-items: center;
    }

    .add-note-btn:hover {
      background-color: #dee0e2;
    }

    .union-icon {
      width: 18.93px;
      height: 18.93px;
      vertical-align: middle;
    }

    .close-icon {
      width: 12px;
      height: 12px;
      padding: 1px 0px 0px 0px;
      gap: 5px;
    }

    .notes-list {
      margin-top: 10px;
      max-height: 200px;
      overflow-y: auto;
    }
    .prescription-list {
      margin-top: 10px;
      max-height: 167px;
      overflow-y: auto;
      display: flex;
      flex-direction: column-reverse;
    }

    .note-item {
      display: flex;
      align-items: center;
      justify-content: space-between;
      background-color: #f7f9fc;
      padding: 10px;
      border: 1px solid #e3e9f1;
      border-radius: 5px;
      margin-bottom: 5px;
    }

    .note-text {
      flex: 1;
      padding: 5px;
      border-radius: 3px;
    }

  

    .note-item:hover {
      background-color: #dee0e2;
    }

    .close-btn {
      background: none;
      border: none;
      color: #707070;
      font-size: 18px;
      cursor: pointer;
    }
    .footer {
      height: 56px;
        box-sizing: border-box;
        display: grid;
        border-top: 2px solid #eaeaea;
        flex-direction: column;
        justify-content: center;

    }

    .footer-text {
      color: #6d6c6c;
        font-size: 11px;
        font-weight: 400;
        line-height: 15px;
        font-style: italic;
        box-sizing: border-box;
    }

    .disclaimer-icon {
      width: 20px;
      height: 20px;
    }

    .transcription-output {
      margin-top: 20px;
      border: 1px solid #ddd;
      padding: 10px;
      min-height: 100px;
      background-color: #f9f9f9;
      border-radius: 5px;
    }
    .header-section .user-menu {
      position: absolute;
      width: 98%;
      display: flex;
      flex-direction: row-reverse;
      top: 11px;
    }
    .patient-info-title {
      position: relative;
      font-weight: 600;
    }
    .patient-info-wrapper {
      align-self: stretch;
      display: flex;
      flex-direction: row;
      align-items: flex-start;
      justify-content: flex-start;
    }
    .patient-info-wrapper h2 {
            color: #1c1c1c;
            font-size: 17px;
            font-style: normal;
            font-weight: 700;
            line-height: normal;
            width: 115%;
    }
    .frame-wrapper {
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      justify-content: flex-start;
    }
    .inputfieldtext {
      display: flex;
      flex-direction: row;
      align-items: flex-start;
      justify-content: flex-start;
    }
    .button-inner {
      display: flex;
      flex-direction: row;
      align-items: center;
      justify-content: flex-start;
    }

    .dob-picker{
      padding: 14px;
      font-size: 14px;
      font-style: normal;
      font-weight: 400;
      color: #8c8c8c !important;
      height: 18px;
      width: 190px;
    }
  
    .input-field {
        align-self: stretch;
        flex: 1;
        border-radius: 4px;
        background-color: #fff;
        border: 1px solid #eee;
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: flex-start;
        padding: 14px;
        color: #353535;
        font-size: 14px;
        font-style: normal;
        font-weight: 400;
        line-height: 150%;
        height: 48px;
    }
    .patient-id-custom {
      position: absolute;
      top: 0px;
      left: 0px;
      width: 171px;
      height: 48px;
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      justify-content: flex-start;
    }
    .group-div {
      position: absolute;
      top: 0px;
      left: 0px;
      width: 171px;
      height: 48px;
    }
    .patient-id-container {
      width: 171px;
      position: relative;
      height: 48px;
      color: #1c1c1c;
    }
    .name {
      position: relative;
      line-height: 150%;
    }
    .input-container {
      position: absolute;
      top: 0px;
      left: 0px;
      width: 287px;
      height: 48px;
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      justify-content: flex-start;
    }
    .name-wrapper {
      position: absolute;
      top: 0px;
      left: 0px;
      width: 287px;
      height: 48px;
    }
    .name-container {
      width: 287px;
      position: relative;
      height: 48px;
    }
    .gender-select {
      position: absolute;
      top: 0px;
      left: 0px;
      width: 222px;
      height: 48px;
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      justify-content: flex-start;
    }
    .gender-select select {
      appearance: none;
      background-image: url("../assets/alt-arrow-down.png");
      background-repeat: no-repeat;
      background-position: right 10px center;
      background-size: 20px;
    }
    .outline-arrows-alt-arrow-d {
      position: absolute;
      top: 13.71px;
      left: 186px;
      width: 24px;
      height: 20.6px;
      overflow: hidden;
    }
    .gender-wrapper {
      position: absolute;
      top: 0px;
      left: 0px;
      width: 222px;
      height: 48px;
    }
    .gender-container {
      width: 222px;
      position: relative;
      height: 48px;
    }

    .age-container {
      position: relative;
      width: 222px;
      height: 48px;
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      justify-content: flex-start;
    }
    .age-wrapper {
      position: absolute;
      top: 0px;
      left: 0px;
      width: 222px;
      height: 48px;
    }

    .age-input{
      display: flex;
     flex-direction: column;
    }
    
   
    .form-group {
      display: flex;
      flex-direction: row;
      align-items: center;
      justify-content: flex-start;
      gap: 30px;
      font-size: 16px;
      color: #bbb;
    }
    .frame-group {
      align-self: stretch;
      display: flex;
      flex-direction: row;
      align-items: center;
      justify-content: space-between;
      gap: 136px;
    }
    .patient-info {
        width: auto;
        height: auto;
        position: relative;
        box-shadow: 0px 4px 14px -1px #e5f1ff;
        border-radius: 12px;
        background-color: #fff;
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        justify-content: flex-start;
        padding: 15px 30px;
        box-sizing: border-box;
        text-align: left;
        font-size: 20px;
        color: #1c1c1c;
        margin-top: 21px;
        margin-right: 1%;
        margin-left: 16px;
    }
    .form-group input:focus {
      outline: none;
    }
    .form-group select:focus {
      outline: none;
    }
    .gender-placeholder {
        font-size: 14px;
        font-style: normal;
        font-weight: 400;
        line-height: 150%; 
    }
    .gender-Opetion-text-color{
      color: #8c8c8c !important;
    }


    @media screen and (min-width: 1026px) and (max-width: 1417px){
      .main-content{
        flex-direction: column;
      }
      .conversations{
        width: auto;
        margin-bottom: 2%;
        margin-right:0px;
      }
      .form-group {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
      }
      .frame-group{
        align-self: stretch;
        display: flex;
        flex-direction: column;
        align-items: stretch;
        justify-content: space-evenly;
        gap: 12px;
    }
    .conversation-text{
      height: 350px;
    }
    }
    .maincontent-container{
      margin-left: 80px;
    }
    @media screen and (min-width: 760px) and (max-width: 1026px){
      .main-content{
        flex-direction: column;
      }
      .conversations{
        width: auto;
        margin-bottom: 2%;
        margin-right:0px;
      }
      .form-group {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
      }
      .frame-group{
        align-self: stretch;
        display: flex;
        flex-direction: column;
        align-items: stretch;
        justify-content: space-evenly;
        gap: 12px;
    }
    .conversation-text{
      height: 280px;
    }
    }

    @media screen and (min-width:420px  ) and (max-width: 760px){
      .main-content{
        flex-direction: column;
      }
      .conversations{
        width: auto;
        margin-bottom: 2%;
        margin-right:0px;
      }
      .form-group {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
      }
      .frame-group{
        align-self: stretch;
        display: flex;
        flex-direction: column;
        align-items: stretch;
        justify-content: space-evenly;
        gap: 12px;
    }
    .conversation-text{
      height: 230px;
    }
    .no-data-found{
      font-size: 18px;
    }
    .patient-info {
      margin-right: 4%;
    }
    .h2-responsive-size{
      font-size: 18px;
    }
    .h3-responsive-size{
      font-size: 16px;
    }
    }

    @media screen and (max-width: 420px){
      .patient-info {
      margin-right: 4%;
    }
      .main-content{
        flex-direction: column;
      }
      .conversations{
        width: auto;
        margin-bottom: 2%;
        margin-right:0px;
        padding: 10px;
      }

      .form-group {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
      }
      .frame-group{
        align-self: stretch;
        display: flex;
        flex-direction: column;
        align-items: stretch;
        justify-content: space-evenly;
        gap: 12px;
    }
    .input-container{
      width: 77%;
    }
    .gender-select{
      width: 77%;
    }
    .gender-Options {
            font-size: 12px;
            font-style: normal;
            font-weight: 400;
            line-height: 150%;
            color: #8c8c8c;
    }
    .conversation-text{
      height: 200px;
    }
    .no-data-found{
      font-size: 16px;
    }
    .h2-responsive-size{
      font-size: 18px;
    }
    .h3-responsive-size{
      font-size: 15px;
    }
    .generate-results.disabled{
      padding: 5px 12px;
      font-size: 12px;
    }
    .generate-results{
      padding: 5px 12px;
      font-size: 12px;
    }
   
    .header-section{
      width: 100%;
      margin-left: 0px;
    }

    .menu{
      top: 92px;
    }
    .maincontent-container{
      margin-left: 0px;
    }
    }

  .potential-diagnosis-toggle{
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  .toggle-container {
    display: flex;
    align-items: center;
    gap: 8px;
  }

  .switch {
  position: relative;
  display: inline-block;
  width: 40px;
  height: 20px;
}

.switch input {
  opacity: 0;
  width: 0;
  height: 0;
}

.slider {
  position: absolute;
  cursor: pointer;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #ccc;
  transition: 0.4s;
  border-radius: 34px;
}

.slider:before {
  position: absolute;
  content: "";
  height: 14px;
  width: 14px;
  left: 3px;
  bottom: 3px;
  background-color: white;
  transition: 0.4s;
  border-radius: 50%;
}

input:checked + .slider {
  background-color: #007bff;
}

input:checked + .slider:before {
  transform: translateX(20px);
}

input:disabled:not(:checked) + .slider {
  background-color: #ccc;
  cursor: not-allowed;
}

input:disabled:not(:checked) + .slider:before {
  background-color: #888;
}

input:disabled:checked + .slider {
  background-color: #80bfff;
  cursor: not-allowed;
}

input:disabled:checked + .slider:before {
  background-color: #ffffff;
}
</style>