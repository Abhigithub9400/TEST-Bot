<template>
  <div class="modal-overlay" v-if="showGenerateResultPreviewPopUp">
    <div class="report-modal">
      <div class="modal-header">
        <h2>Generated Results</h2>
        <div class="button-group-header">
          <button class="action-btn share-btn-common" @click="shareAllReports">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="20"
              height="20"
              viewBox="0 0 20 20"
              fill="none"
            >
              <g clip-path="url(#clip0_3655_1166)">
                <path
                  d="M11.3133 14.3924L7.05492 12.0699C6.64617 12.4757 6.12643 12.7513 5.56125 12.8621C4.99606 12.9729 4.41072 12.9139 3.87903 12.6925C3.34733 12.4711 2.89308 12.0973 2.57355 11.6181C2.25401 11.1389 2.0835 10.5759 2.0835 9.99993C2.0835 9.42399 2.25401 8.86093 2.57355 8.38176C2.89308 7.90258 3.34733 7.52873 3.87903 7.30735C4.41072 7.08596 4.99606 7.02694 5.56125 7.13774C6.12643 7.24853 6.64617 7.52418 7.05492 7.92993L11.3133 5.60743C11.1672 4.92221 11.2727 4.20731 11.6104 3.59347C11.9481 2.97963 12.4955 2.50787 13.1525 2.26446C13.8095 2.02104 14.5321 2.02224 15.1883 2.26783C15.8444 2.51343 16.3902 2.987 16.7259 3.60196C17.0616 4.21692 17.1647 4.93216 17.0164 5.61689C16.8681 6.30162 16.4782 6.91009 15.9182 7.33101C15.3581 7.75194 14.6652 7.9572 13.9663 7.90926C13.2673 7.86132 12.6089 7.56338 12.1116 7.06993L7.85326 9.39243C7.9382 9.79298 7.9382 10.2069 7.85326 10.6074L12.1116 12.9299C12.6089 12.4365 13.2673 12.1385 13.9663 12.0906C14.6652 12.0427 15.3581 12.2479 15.9182 12.6688C16.4782 13.0898 16.8681 13.6982 17.0164 14.383C17.1647 15.0677 17.0616 15.7829 16.7259 16.3979C16.3902 17.0129 15.8444 17.4864 15.1883 17.732C14.5321 17.9776 13.8095 17.9788 13.1525 17.7354C12.4955 17.492 11.9481 17.0202 11.6104 16.4064C11.2727 15.7925 11.1672 15.0776 11.3133 14.3924Z"
                  fill="#3184DD"
                />
              </g>
              <defs>
                <clipPath id="clip0_3655_1166">
                  <rect width="20" height="20" fill="white" />
                </clipPath>
              </defs>
            </svg>
            <p class="text-message">Share</p>
          </button>
          <button
            class="action-btn download-btn-common"
            @click="downloadAllReports"
          >
            Download
          </button>
          <button class="close-btn" @click="closeModal">
            <span>&times;</span>
          </button>
        </div>
      </div>

      <div class="modal-body">
        <div class="result-container">
          <div class="report-section">
            <div class="report-content">
              <div class="hospital-info" id="toRenderinpdf">
                <div class="hospital-logo-container">
                  <img
                    :src="logo"
                    alt="Uploaded Logo Preview"
                    class="hospital-logo"
                  />
                  <div class="hospital-details">
                    <p class="hospital-name">{{ hospitalName }}</p>
                    <p class="hospital-address">{{ hospitalAddress }}</p>
                  </div>
                </div>
                <div class="hospital-details-container">
                  <div class="doctor-name">
                    <p>
                      <strong>{{ doctorName }}</strong>
                    </p>
                  </div>
                  <div class="docspec-date">
                    <p class="doctor-specialty">{{ doctorSpecialty }}</p>
                    <p class="todaysdate">{{ todaysDate }}</p>
                  </div>
                </div>
              </div>
              <div v-if="!editingAll" class="patient-details">
                <h3>Patient Details</h3>
                <button @click="toggleEdit" class="edit-info">
                  <i class="edit-icon"
                    ><img src="@/assets/icons/edit-icon.svg" alt="Edit Icon"
                  /></i>
                </button>
              </div>
              <div v-else class="patient-details report-btn">
                <h3>Patient Details</h3>
                <div class="edit-action-btns">
                  <button class="btn-action cancel-btn" @click="cancelEdit">
                    Cancel
                  </button>
                  <button class="btns-action save-btn" @click="saveAll">
                    Save
                  </button>
                </div>
              </div>
              <table class="patient-info">
                <caption></caption>
                <tr>
                  <th>MRN Number</th>
                  <th>Name</th>
                  <th>Age</th>
                  <th>Gender</th>
                </tr>
                <tr>
                  <td>
                    <span v-if="!editingAll">{{ patientMRN }}</span>
                    <input
                      v-else
                      v-model="editedValues.mrn"
                      class="mrn-input"
                      maxlength="10"
                    />
                  </td>
                  <td>
                    <span v-if="!editingAll">{{ patientName }}</span>
                    <input
                      v-else
                      v-model="editedValues.name"
                      maxlength="20"
                      class="name-input"
                    />
                  </td>
                  <td>
                    <span v-if="!editingAll">{{ patientAge }}</span>
                    <input
                      v-else
                      v-model="editedValues.age"
                      class="age-input"
                      maxlength="3"
                    />
                  </td>
                  <td>
                    <span v-if="!editingAll">{{ patientGender }}</span>
                    <input
                      v-else
                      v-model="editedValues.gender"
                      class="gender-input"
                      maxlength="15"
                    />
                  </td>
                </tr>
              </table>
              <div v-if="!editingSummary" class="summary-report">
                <h3>Summary Report</h3>
                <button @click="toggleSummaryEdit" class="edit-info">
                  <i class="edit-icon"
                    ><img src="@/assets/icons/edit-icon.svg" alt="Edit Icon"
                  /></i>
                </button>
              </div>
              <div v-else class="summary-report report-btn">
                <h3>Summary Report</h3>
                <div class="edit-action-btns">
                  <button
                    class="btn-action cancel-btn"
                    @click="cancelSummaryEdit"
                  >
                    Cancel
                  </button>
                  <button
                    class="btns-action save-btn"
                    @click="saveSummaryChanges"
                  >
                    Save
                  </button>
                </div>
              </div>

              <div
                class="main-content"
                :contenteditable="editingSummary"
                ref="summaryReportRef"
              ></div>

              <div class="signature-section">
                <label for="signature">Signature/Seal</label>
                <div class="signatureBox">
                  <div>
                    <img
                      :src="signature"
                      alt="Uploaded Signature Preview"
                      class="previewImage"
                    />
                  </div>
                </div>
              </div>

              <div class="footer-section" id="pdfFooter">
                <footer class="footer">
                  <p class="footer-text">
                    This medical report is the property of
                    {{ hospitalName }} and is protected under copyright law. Any
                    unauthorized use, reproduction, or distribution is
                    prohibited.
                  </p>
                </footer>
              </div>
            </div>

            <div class="button-group">
              <button class="action-btn share-btn" @click="shareSummaryReport">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="20"
                  height="20"
                  viewBox="0 0 20 20"
                  fill="none"
                >
                  <g clip-path="url(#clip0_3655_1087)">
                    <path
                      d="M11.3133 14.3927L7.05492 12.0702C6.64617 12.4759 6.12643 12.7516 5.56125 12.8624C4.99606 12.9732 4.41072 12.9141 3.87903 12.6928C3.34733 12.4714 2.89308 12.0975 2.57355 11.6183C2.25401 11.1392 2.0835 10.5761 2.0835 10.0002C2.0835 9.42423 2.25401 8.86118 2.57355 8.382C2.89308 7.90282 3.34733 7.52898 3.87903 7.30759C4.41072 7.0862 4.99606 7.02719 5.56125 7.13798C6.12643 7.24878 6.64617 7.52443 7.05492 7.93017L11.3133 5.60767C11.1672 4.92245 11.2727 4.20756 11.6104 3.59372C11.9481 2.97988 12.4955 2.50812 13.1525 2.2647C13.8095 2.02129 14.5321 2.02249 15.1883 2.26808C15.8444 2.51367 16.3902 2.98725 16.7259 3.6022C17.0616 4.21716 17.1647 4.9324 17.0164 5.61713C16.8681 6.30187 16.4782 6.91033 15.9182 7.33126C15.3581 7.75218 14.6652 7.95744 13.9663 7.9095C13.2673 7.86156 12.6089 7.56363 12.1116 7.07017L7.85326 9.39267C7.9382 9.79322 7.9382 10.2071 7.85326 10.6077L12.1116 12.9302C12.6089 12.4367 13.2673 12.1388 13.9663 12.0908C14.6652 12.0429 15.3581 12.2482 15.9182 12.6691C16.4782 13.09 16.8681 13.6985 17.0164 14.3832C17.1647 15.0679 17.0616 15.7832 16.7259 16.3981C16.3902 17.0131 15.8444 17.4867 15.1883 17.7323C14.5321 17.9779 13.8095 17.9791 13.1525 17.7356C12.4955 17.4922 11.9481 17.0205 11.6104 16.4066C11.2727 15.7928 11.1672 15.0779 11.3133 14.3927Z"
                      fill="#3184DD"
                    />
                  </g>
                  <defs>
                    <clipPath id="clip0_3655_1087">
                      <rect width="20" height="20" fill="white" />
                    </clipPath>
                  </defs>
                </svg>
              </button>
              <div class="footer-action-btns">
                <button
                  class="action-btn download-btn"
                  @click="downloadSummaryReport"
                >
                  Download
                </button>
              </div>
            </div>
          </div>

          <div class="report-section">
            <div class="watermark">
              {{ store.MediAssistConfigManager.DomainName }}
            </div>
            <div class="report-content">
              <div class="hospital-info" id="toRenderinpdf">
                <div class="hospital-logo-container">
                  <img
                    :src="logo"
                    alt="Uploaded Logo Preview"
                    class="hospital-logo"
                  />
                  <div class="hospital-details">
                    <p class="hospital-name">{{ hospitalName }}</p>
                    <p class="hospital-address">{{ hospitalAddress }}</p>
                  </div>
                </div>
                <div class="hospital-details-container">
                  <div class="doctor-name">
                    <p>
                      <strong>{{ doctorName }}</strong>
                    </p>
                  </div>
                  <div class="docspec-date">
                    <p class="doctor-specialty">{{ doctorSpecialty }}</p>
                    <p class="todaysdate">{{ todaysDate }}</p>
                  </div>
                </div>
              </div>

              <table class="patient-info">
                <caption></caption>
                <tr>
                  <th>MRN Number</th>
                  <th>Name</th>
                  <th>Age</th>
                  <th>Gender</th>
                </tr>
                <tr>
                  <td>{{ patientMRN }}</td>
                  <td>{{ patientName }}</td>
                  <td>{{ patientAge }}</td>
                  <td>{{ patientGender }}</td>
                </tr>
              </table>
              <div v-if="!editingPrescription" class="prescription-report">
                <h3>Prescription Report</h3>
                <button @click="togglePrescriptionEdit" class="edit-info">
                  <i class="edit-icon"
                    ><img src="@/assets/icons/edit-icon.svg" alt="Edit Icon"
                  /></i>
                </button>
              </div>
              <div v-else class="prescription-report report-btn">
                <h3>Prescription Report</h3>
                <div class="edit-action-btns">
                  <button
                    class="btn-action cancel-btn"
                    @click="cancelPrescriptionEdit"
                  >
                    Cancel
                  </button>
                  <button
                    class="btns-action save-btn"
                    @click="savePrescriptionChanges"
                  >
                    Save
                  </button>
                </div>
              </div>
              <div
                class="prescription-content"
                :contenteditable="editingPrescription"
                ref="prescriptionReportRef"
              ></div>

              <div class="signature-section">
                <label for="signature">Signature/Seal</label>
                <div class="signatureBox">
                  <div>
                    <img
                      :src="signature"
                      alt="Uploaded Signature Preview"
                      class="previewImage"
                    />
                  </div>
                </div>
              </div>

              <div class="footer-section" id="pdfFooter">
                <footer class="footer">
                  <p class="footer-text">
                    This medical report is the property of
                    {{ hospitalName }} and is protected under copyright law. Any
                    unauthorized use, reproduction, or distribution is
                    prohibited.
                  </p>
                </footer>
              </div>
            </div>

            <div class="button-group">
              <button
                class="action-btn share-btn"
                @click="sharePrescriptionReport"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="20"
                  height="20"
                  viewBox="0 0 20 20"
                  fill="none"
                >
                  <g clip-path="url(#clip0_3655_1087)">
                    <path
                      d="M11.3133 14.3927L7.05492 12.0702C6.64617 12.4759 6.12643 12.7516 5.56125 12.8624C4.99606 12.9732 4.41072 12.9141 3.87903 12.6928C3.34733 12.4714 2.89308 12.0975 2.57355 11.6183C2.25401 11.1392 2.0835 10.5761 2.0835 10.0002C2.0835 9.42423 2.25401 8.86118 2.57355 8.382C2.89308 7.90282 3.34733 7.52898 3.87903 7.30759C4.41072 7.0862 4.99606 7.02719 5.56125 7.13798C6.12643 7.24878 6.64617 7.52443 7.05492 7.93017L11.3133 5.60767C11.1672 4.92245 11.2727 4.20756 11.6104 3.59372C11.9481 2.97988 12.4955 2.50812 13.1525 2.2647C13.8095 2.02129 14.5321 2.02249 15.1883 2.26808C15.8444 2.51367 16.3902 2.98725 16.7259 3.6022C17.0616 4.21716 17.1647 4.9324 17.0164 5.61713C16.8681 6.30187 16.4782 6.91033 15.9182 7.33126C15.3581 7.75218 14.6652 7.95744 13.9663 7.9095C13.2673 7.86156 12.6089 7.56363 12.1116 7.07017L7.85326 9.39267C7.9382 9.79322 7.9382 10.2071 7.85326 10.6077L12.1116 12.9302C12.6089 12.4367 13.2673 12.1388 13.9663 12.0908C14.6652 12.0429 15.3581 12.2482 15.9182 12.6691C16.4782 13.09 16.8681 13.6985 17.0164 14.3832C17.1647 15.0679 17.0616 15.7832 16.7259 16.3981C16.3902 17.0131 15.8444 17.4867 15.1883 17.7323C14.5321 17.9779 13.8095 17.9791 13.1525 17.7356C12.4955 17.4922 11.9481 17.0205 11.6104 16.4066C11.2727 15.7928 11.1672 15.0779 11.3133 14.3927Z"
                      fill="#3184DD"
                    />
                  </g>
                  <defs>
                    <clipPath id="clip0_3655_1087">
                      <rect width="20" height="20" fill="white" />
                    </clipPath>
                  </defs>
                </svg>
              </button>
              <div class="footer-action-btns">
                <button
                  class="action-btn download-btn"
                  @click="downloadPrescription"
                >
                  Download
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <ReportSendPopUp
      v-if="!isLoading"
      :is-visible="showReportSendPopUp"
      modal-type="report-send"
      title="Share Medical Report"
      description=" Please enter your name and email address to receive the medical report."
      submitButtonText="Submit"
      @submitReport="sendReport"
      @close="showReportSendPopUp = false"
    />
    <MailSuccessPopup
      :isVisible="showMailSucessPopup"
      message="Report has been successfully shared."
      @cancel="successErrorCancel"
    />
    <MailErrorPopup
      :isVisible="showMailErrorPopup"
      :isError="showMailErrorPopup"
      message="Failed to send the email. Please try again later."
      @cancel="successErrorCancel"
    />
    <LoadingSpinnerVue
      :visible="isLoading"
      message="Sending Report..please wait"
    />
  </div>
</template>

<script setup>
import { ref, onMounted, nextTick, defineProps, defineEmits } from "vue";
import html2pdf from "html2pdf.js";
import html2canvas from "html2canvas";
import ReportSendPopUp from "./GenericModal.vue";
import axiosInstance from "@/Services/Interceptors/axios.js";
import {
  default as MailErrorPopup,
  default as MailSuccessPopup,
} from "./MailSuccessErrorModal.vue";
import LoadingSpinnerVue from "@/components/LoadingSpinner.vue";
import { useMyStore } from "@/store/store.ts";

const props = defineProps({
  reportData: Object,
});
const store = useMyStore();
const showGenerateResultPreviewPopUp = ref(true);
const hospitalName = ref("");
const hospitalAddress = ref("");
const doctorName = ref("");
const doctorSpecialty = ref("");
const date = ref("");
const todaysDate = ref("");
const patientMRN = ref("{{ patientMRN }}");
const patientName = ref("{{ patientName }}");
const patientAge = ref("{{ patientAge }}");
const patientGender = ref("{{ patientGender }}");
const initialDiagnosis = ref("");
const summaryReport = ref("");
const prescriptionReport = ref("");
const signature = ref("");
const logo = ref("");
const emit = defineEmits(["close"]);

const editingSummary = ref(false);
const editingPrescription = ref(false);

const summaryReportRef = ref(null);
const prescriptionReportRef = ref(null);
const showReportSendPopUp = ref(false);
const pdfBlob = ref(null);
const reportName = ref("");
const showMailSucessPopup = ref(false);
const showMailErrorPopup = ref(false);
const isLoading = ref(false);
const userId = ref("");
const medinotexPatientId = ref("");

const editingAll = ref(false);
const patientConsent = ref(false);
const dob = ref("");

const editedValues = ref({
  mrn: "",
  name: "",
  age: "",
  gender: "",
  dob: "",
  patientConsent: "",
});

const toggleEdit = () => {
  editingAll.value = true;
  editedValues.value = {
    mrn: patientMRN.value,
    name: patientName.value,
    age: patientAge.value,
    gender: patientGender.value,
    dob: dob.value,
    patientConsent: patientConsent.value
  };
};

const cancelEdit = () => {
  editingAll.value = false;
};

function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(";").shift();
  return null;
}

onMounted(() => {
  userId.value = getCookie("userId");
});

const saveAll = () => {
  patientMRN.value = editedValues.value.mrn;
  patientName.value = editedValues.value.name;
  patientAge.value = editedValues.value.age;
  patientGender.value = editedValues.value.gender;
  dob.value = editedValues.value.dob;
  patientConsent.value = editedValues.value.patientConsent;
  editingAll.value = false;
  const payload = {
    MrnNumber: patientMRN.value,
    PatientName: patientName.value,
    Gender: patientGender.value,
    Age: patientAge.value,
    PatientId: medinotexPatientId.value,
    UserId: userId.value,
    PatientConsent : patientConsent.value,
    DoB: dob.value,
  };

  axiosInstance.post("/api/patient/insert-update-patient", payload);
};

const closeModal = () => {
  showGenerateResultPreviewPopUp.value = false;
  emit("close");
};

const toggleSummaryEdit = () => {
  editingSummary.value = true;
  if (editingSummary.value) {
    nextTick(() => {
      summaryReportRef.value.focus();
      const range = document.createRange();
      const sel = window.getSelection();
      range.selectNodeContents(summaryReportRef.value);
      range.collapse(false);
      sel.removeAllRanges();
      sel.addRange(range);
    });
  }
};

const saveSummaryChanges = () => {
  if (summaryReportRef.value && editingSummary.value) {
    summaryReport.value = summaryReportRef.value.innerText;
  }
  editingSummary.value = false;
};

const cancelSummaryEdit = () => {
  editingSummary.value = false;
  var previousSummary = summaryReport.value;
  summaryReportRef.value.innerHTML = previousSummary;
};

const cancelPrescriptionEdit = () => {
  editingPrescription.value = false;
  var previousPrescription = prescriptionReport.value;
  prescriptionReportRef.value.innerHTML = previousPrescription;
};

const savePrescriptionChanges = () => {
  if (prescriptionReportRef.value) {
    prescriptionReport.value = prescriptionReportRef.value.innerText;
  }
  editingPrescription.value = false;
};

const togglePrescriptionEdit = () => {
  editingPrescription.value = !editingPrescription.value;
  if (editingPrescription.value) {
    nextTick(() => {
      prescriptionReportRef.value.focus();
      const range = document.createRange();
      const sel = window.getSelection();
      range.selectNodeContents(prescriptionReportRef.value);
      range.collapse(false);
      sel.removeAllRanges();
      sel.addRange(range);
    });
  }
};

const setInitialContent = () => {
  if (summaryReportRef.value) {
    summaryReportRef.value.innerHTML = summaryReport.value;
  }
  if (prescriptionReportRef.value) {
    prescriptionReportRef.value.innerHTML = prescriptionReport.value;
  }
};

const generateDocument = async (options) => {
  const {
    mode = "download",
    type = "summary",
    contentRef,
    patientMRN,
    patientName,
    patientAge,
    patientGender,
    signature,
  } = options;

  const reportContent = document.createElement("div");
  reportContent.innerHTML = `
  <div class="content-section">
    <table class="patient-info">
      <tr>
        <th>MRN Number</th>
        <th>Name</th>
        <th>Age</th>
        <th>Gender</th>
      </tr>
      <tr>
        <td>${patientMRN.value}</td>
        <td>${patientName.value}</td>
        <td>${patientAge.value}</td>
        <td>${patientGender.value}</td>
      </tr>
    </table>
    <h3>${type === "summary" ? "Summary Report" : "Prescription Report"}</h3>
    <div class="summary-content">
      ${contentRef.value.innerHTML}
    </div>
    <div class="signature-section">
      <p>Signature/Seal</p>
      <p>${
        signature.value
          ? `<img src="${signature.value}" alt="Signature" class="signature-image" />`
          : ""
      }</p>
    </div>
  </div>`;

  const style = document.createElement("style");
  style.textContent = `
 
  .patient-info {
  width: 100%;
  border-spacing: 0 10px;
}

.patient-info th,
.patient-info td {
  font-family: Inter, sans-serif;
  font-size: 14px;
  line-height: 21px;
  text-align: left;
  vertical-align: top;
}
.patient-info th {
  font-weight: 600;
  color: var(--Text-light, #5a5a5a);
}
.patient-info td {
  font-weight: 400;
  color: #1c1c1c;
}
  .signature-section {
    margin-top: 20px;
    display: flex;
    align-items: center;
    gap: 10px;
  }
  .signature-image {
    width: 200px;
    height: 47px;
    flex-shrink: 0;
    border-radius: 3.168px;
    border: 0.792px dashed rgba(0, 102, 212, 0.30);
    background: #EEF6FF;
    justify-items: center;
  }

    h3 {
      font-family: Inter, sans-serif;
      font-size: 20px;
      font-weight: 600;
      color: #1c1c1c;
}

  .newsec {
    position: fixed;
    margin-top: 75px;
    top: 10;
    height: 100%;
    width: 100%;
  }
  .html2pdf__page-break {
    page-break-before: always;
    page-break-after: avoid;
    page-break-inside: avoid;
  }
  .content-section {
    padding: 20px 20px 25px 20px;
    
  }
`;

  reportContent.appendChild(style);

  const pdfOptions = {
    margin: [45, 10, 27.5, 10],
    image: { type: "jpeg", quality: 0.98 },
    html2canvas: { scale: 2, logging: true },
    jsPDF: { unit: "mm", format: "a4", orientation: "portrait" },
  };

  const headerElement = document.getElementById("toRenderinpdf");
  const footerElement = document.getElementById("pdfFooter");

  const headerCanvas = await html2canvas(headerElement);
  const headerImage = headerCanvas.toDataURL("image/png");

  const footerCanvas = await html2canvas(footerElement);
  const footerImage = footerCanvas.toDataURL("image/png");

  const pdf = await html2pdf()
    .set(pdfOptions)
    .from(reportContent)
    .toPdf()
    .get("pdf");
  const totalPages = pdf.internal.getNumberOfPages();
  const pageWidth = pdf.internal.pageSize.width;
  const pageHeight = pdf.internal.pageSize.height;

  const footerHeight = 22;
  const headerHeight = 40;

  for (let i = 1; i <= totalPages; i++) {
    pdf.setPage(i);
    pdf.addImage(headerImage, "PNG", 0.5, 0.5, pageWidth - 1, headerHeight);

    pdf.addImage(
      footerImage,
      "PNG",
      0.5,
      pageHeight - footerHeight,
      pageWidth - 1,
      footerHeight
    );

    pdf.setFontSize(60);
    pdf.setFont("Arial", "normal");
    pdf.setTextColor(128, 128, 128);
    pdf.setGState(new pdf.GState({ opacity: 0.5 }));
    pdf.text(
      `${store.MediAssistConfigManager.DomainName.toUpperCase()}`,
      pageWidth / 1.5,
      pageHeight / 1.5,
      {
        angle: 45,
        align: "center",
      }
    );
    pdf.setGState(new pdf.GState({ opacity: 1 }));
  }
  const filename = `${patientName.value}_${type}_report.pdf`;
  if (mode === "share") {
    const data = pdf.output("blob");
    pdfBlob.value = await convertBlobToBase64(data);
    return data;
  } else if (mode === "download") {
    pdf.save(filename);
  }
};

const convertBlobToBase64 = (blob) => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(blob);
    reader.onloadend = () => {
      const base64data = reader.result.split(",")[1];
      resolve(base64data);
    };
    reader.onerror = (error) => reject(error);
  });
};

const sendReport = async ({ email, name }) => {
  isLoading.value = true;
  try {
    const formData = new FormData();
    formData.append("file", pdfBlob.value);
    formData.append("Email", email);
    formData.append("RecipientName", name);
    formData.append("HospitalName", props.reportData.hospitalInfo.hospitalName);
    formData.append(
      "HospitalAddress",
      props.reportData.hospitalInfo.hospitalAddress
    );
    formData.append("ReportName", reportName.value);
    formData.append("PatientName", patientName.value);
    formData.append("DoctorName", doctorName.value);
    formData.append("ConsultationDate", date.value.toISOString().split("T")[0]);

    const response = await axiosInstance.post(
      "/api/manage/shareRreport",
      formData,
      {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      }
    );

    if (response.status === 200 && response.data.success) {
      showMailSucessPopup.value = true;
      showReportSendPopUp.value = false;
    } else {
      showMailErrorPopup.value = true;
      showReportSendPopUp.value = false;
    }
  } catch (error) {
    showMailErrorPopup.value = true;
    showReportSendPopUp.value = false;
  } finally {
    isLoading.value = false;
  }
};

const downloadSummaryReport = () => {
  generateDocument({
    mode: "download",
    type: "summary",
    contentRef: summaryReportRef,
    hospitalName,
    hospitalAddress,
    doctorName,
    doctorSpecialty,
    patientMRN,
    patientName,
    patientAge,
    patientGender,
    signature,
  });
};

const downloadPrescription = () => {
  generateDocument({
    mode: "download",
    type: "prescription",
    contentRef: prescriptionReportRef,
    hospitalName,
    hospitalAddress,
    doctorName,
    doctorSpecialty,
    patientMRN,
    patientName,
    patientAge,
    patientGender,
    signature,
  });
};

const shareSummaryReport = () => {
  reportName.value = "Summary Report";
  generateDocument({
    mode: "share",
    type: "summary",
    contentRef: summaryReportRef,
    hospitalName,
    hospitalAddress,
    doctorName,
    doctorSpecialty,
    patientMRN,
    patientName,
    patientAge,
    patientGender,
    signature,
  });
  showReportSendPopUp.value = true;
};
const sharePrescriptionReport = () => {
  reportName.value = "Prescription Report";
  generateDocument({
    mode: "share",
    type: "prescription",
    contentRef: prescriptionReportRef,
    hospitalName,
    hospitalAddress,
    doctorName,
    doctorSpecialty,
    patientMRN,
    patientName,
    patientAge,
    patientGender,
    signature,
  });
  showReportSendPopUp.value = true;
};

const generateAllReports = async (options) => {
  const { action = "download" } = options;
  const createReportPage = (title, contentRef) => `
        <div class="content-section">
            <table class="patient-info">
                <tr>
                    <th>MRN Number</th>
                    <th>Name</th>
                    <th>Age</th>
                    <th>Gender</th>
                </tr>
                <tr>
                    <td>${patientMRN.value}</td>
                    <td>${patientName.value}</td>
                    <td>${patientAge.value}</td>
                    <td>${patientGender.value}</td>
                </tr>
            </table>
            <h3>${title}</h3>
            <div class="report-content">
                ${contentRef.value.innerHTML}
            </div>
            <div class="signature-section">
                <p>Signature/Seal</p>
                ${
                  signature.value
                    ? `<img src="${signature.value}" alt="Signature" class="signature-image" />`
                    : ""
                }
            </div>
        </div>
    `;

  const combinedContent = document.createElement("div");
  combinedContent.style.margin = "0";
  combinedContent.style.padding = "0";

  combinedContent.innerHTML = `
        <div>
            ${createReportPage("Summary Report", summaryReportRef)}
            <div class="page-break"></div>
            ${createReportPage("Prescription Report", prescriptionReportRef)}
        </div>
    `;

  const style = document.createElement("style");
  style.textContent = `
        .patient-info {
            width: 100%;
            border-spacing: 0 10px;
        }

        .patient-info th,
        .patient-info td {
            font-family: Inter, sans-serif;
            font-size: 14px;
            line-height: 21px;
            text-align: left;
            vertical-align: top;
        }
        .patient-info th {
            font-weight: 600;
            color: var(--Text-light, #5a5a5a);
        }
        .patient-info td {
            font-weight: 400;
            color: #1c1c1c;
        }

        .signature-section {
            margin-top: 20px;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .signature-image {
            width: 200px;
            height: 47px;
            flex-shrink: 0;
            border-radius: 3.168px;
            border: 0.792px dashed rgba(0, 102, 212, 0.30);
            background: #EEF6FF;
            justify-items: center;
        }

        h3 {
            font-family: Inter, sans-serif;
            font-size: 20px;
            font-weight: 600;
            color: #1c1c1c;
            margin-bottom: 15px;
        }

        .content-section {
            padding: 20px 20px 25px 20px;
        }

        .page-break {
            page-break-before: always;
        }
    `;

  combinedContent.appendChild(style);

  const pdfOptions = {
    margin: [45, 10, 28.5, 10],
    image: { type: "jpeg", quality: 0.98 },
    html2canvas: { scale: 2, logging: true },
    jsPDF: { unit: "mm", format: "a4", orientation: "portrait" },
  };

  const headerElement = document.getElementById("toRenderinpdf");
  const footerElement = document.getElementById("pdfFooter");

  const headerCanvas = await html2canvas(headerElement);
  const headerImage = headerCanvas.toDataURL("image/png");

  const footerCanvas = await html2canvas(footerElement);
  const footerImage = footerCanvas.toDataURL("image/png");

  const pdf = await html2pdf()
    .set(pdfOptions)
    .from(combinedContent)
    .toPdf()
    .get("pdf");
  const totalPages = pdf.internal.getNumberOfPages();
  const pageWidth = pdf.internal.pageSize.width;
  const pageHeight = pdf.internal.pageSize.height;

  const footerHeight = 22;
  const headerHeight = 40;

  for (let i = 1; i <= totalPages; i++) {
    pdf.setPage(i);

    pdf.addImage(headerImage, "PNG", 0.5, 0.5, pageWidth - 1, headerHeight);

    pdf.addImage(
      footerImage,
      "PNG",
      0.5,
      pageHeight - footerHeight,
      pageWidth - 1,
      footerHeight
    );
    // Add watermark
    pdf.setFontSize(60);
    pdf.setFont("Arial", "normal");
    pdf.setTextColor(150, 150, 150);
    pdf.setGState(new pdf.GState({ opacity: 0.5 }));
    pdf.text(
      `${store.MediAssistConfigManager.DomainName.toUpperCase()}`,
      pageWidth / 1.5,
      pageHeight / 1.5,
      {
        angle: 45,
        align: "center",
      }
    );
    pdf.setGState(new pdf.GState({ opacity: 1 }));
  }
  const filename = `${patientName.value}_ComprehensiveReport.pdf`;

  if (action === "share") {
    const data = pdf.output("blob");
    pdfBlob.value = await convertBlobToBase64(data);
    return data;
  } else if (action === "download") {
    pdf.save(filename);
  }
};

const downloadAllReports = () => {
  generateAllReports({
    action: "download",
  });
};

const shareAllReports = () => {
  reportName.value = "ComprehensiveReport";
  generateAllReports({
    action: "share",
  });
  showReportSendPopUp.value = true;
};

onMounted(async () => {
  const data = await fetchData();
  hospitalName.value = data.hospitalName;
  hospitalAddress.value = data.hospitalAddress;
  doctorName.value = data.doctorName;
  doctorSpecialty.value = data.doctorSpecialty;
  patientMRN.value = data.patientMRN;
  patientName.value = data.patientName;
  patientAge.value = data.patientAge;
  patientGender.value = data.patientGender;
  initialDiagnosis.value = data.initialDiagnosis;
  summaryReport.value = data.summaryReport;
  prescriptionReport.value = data.prescriptionReport;
  signature.value = data.doctorSignature;
  logo.value = data.hospitalLogo;
  date.value = new Date();
  todaysDate.value = date.value.toLocaleString().replace(/,/g, "");
  medinotexPatientId.value = data.medinotexPatientId;

  nextTick(() => {
    setInitialContent();
  });
});

const fetchData = async () => {
  const doctorTitleAndName =
    props.reportData.hospitalInfo.doctorTitle +
    " " +
    props.reportData.hospitalInfo.doctorName;
  return {
    hospitalName: props.reportData.hospitalInfo.hospitalName,
    hospitalAddress: props.reportData.hospitalInfo.hospitalAddress,
    doctorName: doctorTitleAndName,
    doctorSpecialty: props.reportData.hospitalInfo.doctorSpecialty,
    doctorSignature: props.reportData.hospitalInfo.doctorSignature,
    hospitalLogo: props.reportData.hospitalInfo.hospitalLogo,
    patientMRN: props.reportData.patientInfo.mrn,
    patientName: props.reportData.patientInfo.name,
    patientAge: props.reportData.patientInfo.age,
    patientGender: props.reportData.patientInfo.gender,
    medinotexPatientId: props.reportData.patientInfo.medinotexPatientId,
    summaryReport: `
      ${renderSection(
        "Chief Complaint",
        props.reportData.reports.summary.cheifComplaint
      )}
      ${renderSection(
        "Medical History",
        props.reportData.reports.summary.medicalHistory
      )}
      ${renderSection(
        "Observation",
        props.reportData.reports.summary.observation
      )}
      ${renderSection(
        "Treatment Plan",
        props.reportData.reports.summary.treatmentPlan
      )}
    `,
    prescriptionReport: formatPrescriptionReport(
      props.reportData.reports.prescriptions
    ),
  };
};

const renderSection = (title, content) => {
  return content ? `<p><strong>${title}:</strong></p><p>${content}</p>` : "";
};

/*************  ✨ Codeium Command ⭐  *************/
/**
 * Formats the prescription report into HTML.
 * The report is divided into two sections: medications and laboratory tests.
 * The medications section is displayed in a table with the following columns:
 *   - Name
 *   - Dosage
 *   - Frequency
 *   - Duration
 * The laboratory tests section is displayed in an unordered list with the
 * test name as the list item.
 * @param {Object} prescriptionReport The prescription report data.
 * @returns {string} The formatted HTML string.
 */
/******  32416f04-71ed-4170-86e5-3f28d73cad51  *******/
const formatPrescriptionReport = (prescriptionReport) => {
  let prescriptionHTML = "";

  // Prescription-style table for Medications
  if (
    prescriptionReport.Medications &&
    prescriptionReport.Medications.length > 0
  ) {
    prescriptionHTML += "<strong>Medications:</strong>";
    prescriptionHTML += `
      <table style="
        width: 100%; 
        border-collapse: collapse; 
        margin-top: 10px; 
        font-family: Arial, sans-serif; 
        font-size: 14px;
      ">
        <thead>
          <tr style="background-color: #f2f2f2; text-align: left;">
            <th style="border: 1px solid #ddd; padding: 8px;">Name</th>
            <th style="border: 1px solid #ddd; padding: 8px;">Dosage</th>
            <th style="border: 1px solid #ddd; padding: 8px;">Frequency</th>
            <th style="border: 1px solid #ddd; padding: 8px;">Duration</th>
          </tr>
        </thead>
        <tbody>`;

    prescriptionReport.Medications.forEach((med) => {
      prescriptionHTML += `
        <tr>
          <td style="border: 1px solid #ddd; padding: 8px;">${
            med.Name || ""
          }</td>
          <td style="border: 1px solid #ddd; padding: 8px;">${
            med.Dosage || ""
          }</td>
          <td style="border: 1px solid #ddd; padding: 8px;">${
            med.Frequency || ""
          }</td>
          <td style="border: 1px solid #ddd; padding: 8px;">${
            med.Duration || ""
          }</td>
        </tr>`;
    });

    prescriptionHTML += `
        </tbody>
      </table><br><br>`;
  }

  // Laboratory Tests list
  if (
    prescriptionReport.Laboratory_Tests &&
    prescriptionReport.Laboratory_Tests.length > 0
  ) {
    prescriptionHTML +=
      '<strong>Laboratory Tests:</strong><ul style="margin-top: 10px; font-family: Arial, sans-serif; font-size: 14px;">';

    prescriptionReport.Laboratory_Tests.forEach((test) => {
      prescriptionHTML += `<li>${test.test}</li>`;
    });

    prescriptionHTML += "</ul>";
  }

  return prescriptionHTML;
};

const successErrorCancel = () => {
  showMailSucessPopup.value = false;
  showMailErrorPopup.value = false;
};
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}

.report-modal {
  background: #ffffff;
  border-radius: 10px;
  width: 80%;
  height: 90%;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px;
}

.modal-header h2 {
  margin: 0;
  font-family: Inter, sans-serif;
  font-size: 24px;
  font-weight: 600;
  color: #1c1c1c;
}

.button-group-header {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  align-items: center;
}

.footer-action-btns {
  display: flex;
  gap: 115px;
}

.edit-action-btns {
  display: flex;
  gap: 10px;
}

.close-btn {
  background: none;
  border: none;
  font-size: 2.5em;
  cursor: pointer;
  color: #aaa;
}

.close-btn:hover {
  color: black;
}

.modal-body {
  flex-grow: 1;
  overflow-y: auto;
  padding: 0 20px;
}

.result-container {
  display: flex;
  justify-content: space-between;
  gap: 20px;
  height: 100%;
  flex-wrap: wrap;
}

.report-section {
  width: calc(50% - 10px);
  border-radius: 10px;
  background: #f6faff;
  border: 1px solid #eeeeee;
  display: flex;
  flex-direction: column;
  height: calc(100% - 40px);
}

.report-content {
  flex-grow: 1;
  overflow-y: auto;
  -ms-overflow-style: none;
  scrollbar-width: thin;
  scrollbar-color: #888 transparent;
  padding: 20px;
  height: calc(100% - 60px);
}

.hospital-info {
  align-items: center;
  background: #e6f1ff;
  border-radius: 10px 10px 0 0;
  padding: 10px;
  margin-bottom: 20px;
}

.hospital-logo {
  width: 67px;
  height: auto;
  border-radius: 7px;
  border: 1px solid #e9e9e9;
}

.hospital-details,
.doctor-name {
  flex: 1;
}

.hospital-details {
  flex-grow: 1;
}

.doctor-specialty {
  font-size: 14px !important;
  margin: 0px !important;
  margin-top: 4px !important;
}

.doctor-name {
  font-size: 16px !important;
}

.docspec-date {
  display: flex;
  justify-content: space-between;
}

.todaysdate {
  font-size: 13px !important;
  line-height: 18px !important;
  text-align: center;
  margin: 0px !important;
  margin-top: -15px !important;
  width: 78px;
}

.hospital-name,
.hospital-address,
.doctor-name p {
  margin: 0;
  font-family: Lato, sans-serif;
  line-height: 24px;
}

.hospital-name {
  font-size: 16px;
  font-weight: 500;
}

.hospital-address,
.doctor-name p {
  font-size: 14px;
  font-weight: 400;
}

.hospital-address {
  font-size: 12px !important;
  line-height: 20px !important;
}

.patient-info {
  width: 100%;
  border-collapse: separate;
  border-spacing: 0 10px;
  margin-bottom: 20px;
}

.patient-info th,
.patient-info td {
  font-size: 14px;
  line-height: 21px;
  text-align: left;
  vertical-align: top;
}

.patient-info th {
  font-weight: 600;
  color: var(--Text-light, #5a5a5a);
  padding-right: 16px;
  padding: 0 10px;
}

.patient-info td {
  font-weight: 400;
  color: #1c1c1c;
  padding-right: 16px;
  padding: 0 10px;
}

.diagnosis {
  margin-bottom: 20px;
}

.diagnosis span {
  font-family: Inter, sans-serif;
  font-size: 14px;
  font-weight: 400;
  color: #5a5a5a;
}

.diagnosis p {
  font-family: Lato, sans-serif;
  font-size: 14px;
  font-weight: 400;
  color: #1c1c1c;
  margin-top: 5px;
}

h3 {
  font-family: Inter, sans-serif;
  font-size: 20px;
  font-weight: 600;
  color: #1c1c1c;
  margin-bottom: 15px;
}

.main-content,
.prescription-content {
  font-family: Inter, sans-serif;
  font-size: 14px;
  line-height: 21px;
  color: #353535;
}

.button-group {
  display: flex;
  justify-content: space-between;
  gap: 115px;
  padding: 10px 20px;
  background-color: #f6faff;
}

.action-btn {
  padding: 10px 20px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-family: Inter, sans-serif;
  font-size: 14px;
  font-weight: 500;
}

.btn-action {
  display: flex;
  height: 36px;
  padding: 8px 20px;
  justify-content: center;
  align-items: center;
  gap: 8px;
  border-radius: 8px;
  border: 1px solid #d9d9d9;
  background: #fff;
}

.cancel-btn {
  color: #1c1c1c;
  font-family: Inter;
  font-size: 14px;
  font-style: normal;
  font-weight: 500;
  line-height: 150%;
  cursor: pointer;
}

.cancel-btn:hover {
  background-color: #f4f4f4;
}
.save-btn {
  color: var(--Text-Text-white, #fff);
  font-family: Inter;
  font-size: 14px;
  font-style: normal;
  font-weight: 500;
  line-height: 150%;
  cursor: pointer;
}

.btns-action {
  display: inline-flex;
  height: 36px;
  padding: 8px 20px;
  justify-content: center;
  align-items: center;
  gap: 8px;
  border-radius: 8px;
  background: #0066d4;
  border: none;
}

.edit-btn {
  background: #ffffff;
  border: 1px solid #0066d4;
  color: #0066d4;
  margin-right: -103px;
}

.edit-btn:hover {
  background-color: #f4f4f4;
}

.download-btn {
  background-color: #007bff;
  color: white;
}

.download-btn:hover {
  background-color: #005bb5;
}

.share-btn-common {
  display: inline-flex;
  height: 42px;
  padding: 8px 20px;
  justify-content: center;
  align-items: center;
  gap: 8px;
  flex-shrink: 0;
  border-radius: 8px;
  border: 1px solid #0066d4;
  background: #fff;
}
.share-btn:hover {
  background-color: #f4f4f4;
}
.share-btn-common:hover {
  background-color: #f4f4f4;
}
.share-btn {
  display: flex;
  height: 48px;
  padding: 8px 16px;
  justify-content: center;
  align-items: center;
  gap: 8px;
  border-radius: 8px;
  border: 1px solid #0066d4;
  background: #fff;
  margin-right: 265px;
}

.download-btn-common {
  background-color: #2eb65c;
  border: 1px solid #2eb65c;
  color: white;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-family: Inter, sans-serif;
  font-size: 14px;
  font-weight: 500;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.download-btn-common:hover {
  background-color: #47d075;
}

.signature-section {
  margin-top: 20px;
  display: flex;
  align-items: center;
  gap: 10px;
}

.signature-section label {
  font-family: Inter, sans-serif;
  font-size: 14px;
}

.signature-section input {
  width: 200px;
  height: 40px;
  border: 1px solid #ededed;
  border-radius: 5px;
  padding: 0 10px;
}

.disclaimer-icon {
  width: 20px;
  height: 20px;
  vertical-align: middle;
  margin-right: 5px;
}

.footer-section {
  border-top: 1px solid #eaeaea;
  margin-top: 20px;
}

.footer {
  background-color: #f3f9ff;
  padding: 15px;
  text-align: center;
}

.footer-text {
  color: #6d6c6c;
  font-size: 10px;
  font-weight: 100;
  font-style: italic;
  margin: 0;
}

.watermark {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%) rotate(-45deg);
  font-size: 60px;
  color: rgba(128, 128, 128, 0.15);
  white-space: nowrap;
  pointer-events: none;
  z-index: 1000;
  text-transform: uppercase;
  letter-spacing: 5px;
  width: 100%;
  text-align: center;
}

.main-content[contenteditable="true"],
.prescription-content[contenteditable="true"] {
  border: 1px solid #ccc;
  padding: 10px;
  border-radius: 5px;
  outline: none;
}

.main-content[contenteditable="true"]:focus,
.prescription-content[contenteditable="true"]:focus {
  border-color: #007bff;
  box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
}
input {
  min-width: 50px;
  max-width: 100%;
  box-sizing: border-box;
  border-radius: 3px;
  border: white;
  outline: none;
}
td {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
.edit-info {
  background: none;
  border: none;
  margin-bottom: -10px;
}
.hospital-details-container {
  margin-top: 20px;
  gap: 4px;
}
.hospital-logo-container {
  display: flex;
  gap: 1rem;
  width: 76%;
}
@media (max-width: 575px) {
  .result-container {
    flex-direction: row;
  }
  .report-section {
    width: 100%;
  }
  .report-modal {
    width: 90%;
  }
  .modal-header h2 {
    font-size: 20px !important;
  }
  .report-content {
    padding: 8px;
  }

  .hospital-logo-container {
    flex-wrap: wrap;
    flex-direction: column;
  }
  .doctor-name {
    margin-left: 0px;
    margin-top: 5%;
  }
}
@media (min-width: 576px) and (max-width: 767px) {
  .result-container {
    flex-direction: row;
  }
  .report-section {
    width: 100%;
  }
  .report-modal {
    width: 90%;
  }
  .modal-header h2 {
    font-size: 20px !important;
  }
  .report-content {
    padding: 8px;
  }

  .hospital-logo-container {
    flex-wrap: wrap;
    flex-direction: row;
  }
  .doctor-name {
    margin-top: 0px;
  }
}
@media (min-width: 768px) and (max-width: 991px) {
  .result-container {
    flex-direction: row;
  }
  .report-section {
    width: 100%;
  }
  .report-modal {
    width: 90%;
  }
  .modal-header h2 {
    font-size: 20px !important;
  }
  .report-content {
    padding: 8px;
  }

  .hospital-logo-container {
    flex-wrap: wrap;
    flex-direction: row;
  }
  .doctor-name {
    margin-top: 0px;
  }
}

@media (min-width: 992px) {
  .result-container {
    flex-direction: row;
    flex-wrap: nowrap;
  }
  .report-section {
    width: 100%;
  }
  .report-modal {
    width: 90%;
  }

  .report-content {
    padding: 8px;
  }

  .hospital-logo-container {
    flex-wrap: nowrap;
    flex-direction: row;
  }
  .doctor-name {
    margin-top: 0px;
  }
}

.hiddenFileInput {
  display: none;
}

.signatureBox {
  width: 250px;
  height: 100px;
  flex-shrink: 0;
  border-radius: 3.168px;
  border: 0.792px dashed rgba(0, 102, 212, 0.3);
  background: #eef6ff;
  justify-items: center;
}

.previewImage {
  border-radius: 3px;
  width: 250px;
  height: 100px;
}
.text-message {
  color: #0066d4;
  font-size: 16px;
  font-style: normal;
  font-weight: 600;
  line-height: 150%;
}

.patient-details {
  align-items: center;
  align-self: stretch;
  display: flex;
  gap: 16px;
  flex: 0 0 auto;
  position: relative;
}

.summary-report {
  align-items: center;
  align-self: stretch;
  display: flex;
  gap: 16px;
  flex: 0 0 auto;
  position: relative;
}

.prescription-report {
  align-items: center;
  align-self: stretch;
  display: flex;
  gap: 16px;
  flex: 0 0 auto;
  position: relative;
}

.report-btn {
  justify-content: space-between;
}

.mrn-input {
  border-radius: 8px;
  border: 1px solid #e7e9ec;
  background: #fff;
  display: flex;
  width: 122px;
  padding: 4px 8px;
  align-items: center;
  gap: 10px;
  width: 122px;
  height: 29px;
}

.name-input {
  border-radius: 8px;
  border: 1px solid #e7e9ec;
  background: #fff;
  display: flex;
  padding: 4px 8px;
  align-items: center;
  gap: 10px;
  align-self: stretch;
  width: 256.58936px;
  height: 29px;
}

.age-input {
  border-radius: 8px;
  border: 1px solid #e7e9ec;
  background: #fff;
  display: flex;
  padding: 4px 8px;
  flex-direction: column;
  justify-content: center;
  align-items: flex-start;
  gap: 10px;
  align-self: stretch;
  width: 43px;
  height: 29px;
}

.gender-input {
  border-radius: 8px;
  border: 1px solid #e7e9ec;
  background: #fff;
  display: flex;
  padding: 4px 8px;
  align-items: center;
  gap: 10px;
  align-self: stretch;
  width: 106.41064px;
  height: 29px;
}
</style>
