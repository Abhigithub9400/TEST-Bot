<template>
    <div class="container">
        <form @submit.prevent="handleSave">
            <div class="docInfo-container">
                <div class="docInfo-title">
                    Doctor's Information
                </div>
                <div class="fieldTexts">
                    Signature<span class="mandatorySymbol">*</span>
                </div>
                <diV v-if="signImage" class="delete-icon" @click="deleteSignImage">
                            <img src="@/assets/icons/image-close-icon.svg" alt="Delete Icon" class="trashIcon" />
                        </diV>
                <div>
                    <div class="signatureBox" 
                    @dragover.prevent
                    @dragenter.prevent
                    @drop.prevent="handleFileDrop"
                    >

                        <input 
                        type="file" 
                        ref="fileInput"
                        class="hiddenFileInput"
                        accept="image/jpeg, image/png"
                        @change="handleFileSelect" />

                        <div class="uploadIcon" v-if="!signImage">
                            <img src="@/assets/sign-upload-icon.png" alt="Upload Icon" />
                        </div>
                        <div class="dragAndDrop" v-if="!signImage">
                            Drag & drop files or <span class="browseText" @click="browseFile">Browse</span>
                        </div>
                        <div class="supportedText" v-if="!signImage">
                            Supported formates: JPEG, PNG
                        </div>
                        <div v-if="signImage" class="imagePreview">
                            <img :src="signImage" alt="Uploaded Signature Preview" class="previewImage"/>
                        </div>
                        
                    </div>
                    <span class="error-image-upload" v-if="isDocSignError">{{ signatureError }}</span>
                </div>
                <div class="nameField">
                    <div class="fieldTexts">
                        <label for="fullName">Full Name<span class="mandatorySymbol">*</span></label>
                    </div>
                    <div>
                        <input 
                            type="text" 
                            name="fullName" 
                            id="fullName" 
                            v-model="fullName"
                            class="inputTextBoxes"
                            disabled
                        />
                    </div>
                </div>
                <div class="medicalCredSpecGroup">
                    <div>
                        <div class="fieldTexts">
                            <label for="medCred">Medical Credentials<span class="mandatorySymbol">*</span></label>
                        </div>
                        <div>
                            <select
                                name="medCred"
                                id="medCred"
                                class="medCredDropDownBox"
                                v-model.number="medCred"
                                @blur="validateField('medCred')"
                            >
                                <option :value="1">MD (Doctor of Medicine)</option>
                                <option :value="2">MBBS (Bachelor of Medicine, Bachelor of Surgery)</option>
                                <option :value="3">DO (Doctor of Osteopathic Medicine)</option>
                                <option :value="4">BDS (Bachelor of Dental Surgery)</option>
                                <option :value="5">MCh (Master of Surgery)</option>
                                <option :value="6">DM (Doctorate of Medicine)</option>
                                <option :value="7">FRCS (Fellowship of the Royal College of Surgeons)</option>
                                <option :value="8">FACP (Fellow of the American College of Physicians)</option>
                                <option :value="9">MS (Master of Surgery)</option>
                                <option :value="10">DNB (Diplomate of National Board)</option>
                            </select>
                        </div>
                        <span v-if="errors.medCred" class="error">
                        {{ errors.medCred }}
                        </span>
                    </div>
                    <div>
                        <div class="fieldTexts">
                            <label for="specialization">Specialization<span class="mandatorySymbol">*</span></label>
                        </div>
                        <div>
                            <input 
                                type="text"
                                name="specialization"
                                id="specialization"
                                v-model="specialization" 
                                @blur="validateField('specialization')"
                                @input="validateSpecialization"
                                class="inputTextBoxes"/>
                        </div>
                        <span v-if="errors.specialization" class="error">
                        {{ errors.specialization }}
                        </span>
                    </div>
                </div>
            </div>

            <div class="btnSession">
                <button type="button" @click="cancelDocUpdateAction" class="cancel-btn">
                    Cancel
                </button>
                <button type="submit" class="save-btn">Save</button>
            </div>
        </form>
    </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axiosInstance from '@/Services/Interceptors/axios.js';

const userId = ref("");
const fullName = ref("");
const medCred = ref("");
const specialization = ref("");
const message = ref("");
const success = ref(false);
const docSign = ref(null);
const signImage = ref("");
const signature = ref("");
const isDocSignError = ref(false);
const signatureError = ref("");
const errors = ref({
  medCred: "",
  specialization: "",
});

function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
  return null;
}

onMounted(() => {
    userId.value = getCookie("userId");
    getDocterDetails(userId.value);
});

const getDocterDetails = async (userId) => {

  try {
    const response = await axiosInstance.get(`/api/profile/get?UserId=${userId}`);
    fullName.value = response.data.fullName;
    medCred.value = response.data.medicalCredentials;
    specialization.value = response.data.specialization;

    let imageType = "png";
    if (response.data.signature != null) {
      if (response.data.signature.startsWith("/9j")) {
        imageType = "jpeg";
      }
      signature.value = `data:image/${imageType};base64,${response.data.signature}`;
      signImage.value = signature.value;
      docSign.value = signature.value;
    } else {
      signImage.value = "";
      docSign.value = "";
    }
    
  } catch (err) {
    console.log("An error occurred while fetching user data.");
  }
};

const browseFile = () => {
    document.querySelector(".hiddenFileInput").click();
};

const handleFileSelect = (event) => {
    const file = event.target.files[0];
    validateAndProcessFile(file);
    event.target.value = "";
};

const handleFileDrop = (event) => {
    const file = event.dataTransfer.files[0];
    validateAndProcessFile(file);
    event.dataTransfer.value = "";
};

const validateAndProcessFile = (file) => {

  if (file) {
    const fileExtension = file.name.split(".").pop().toLowerCase();
    if (fileExtension === "png" || fileExtension === "jpg") {
      const reader = new FileReader();
      reader.onload = (e) => {
        signImage.value = e.target.result;
        docSign.value = e.target.result;
        isDocSignError.value = false;
        signatureError.value = "";
      };
      reader.readAsDataURL(file);
    } else {
        signatureError.value = "Please upload a valid file format";
        isDocSignError.value = true;
      setTimeout(() => {
        isDocSignError.value = false;
        signatureError.value = "";
    }, 3000);
    }
  }
};

const deleteSignImage = () => {
    signImage.value = "";  // Clear image preview
    docSign.value = "";    // Clear stored value
};

const cancelDocUpdateAction = () => {
    errors.value.medCred = "";
    errors.value.specialization = "";
    signatureError.value = "";
    isDocSignError.value = false;
    scrollToTop();
    getDocterDetails(userId.value);
};

const validateSpecialization = (event) => {
  const regex = /^[A-Za-z\s]*$/;
  const inputValue = event.target.value;
  if (regex.test(inputValue)) {
    specialization.value = inputValue;
  } else {
    const sanitizedValue = inputValue.replace(/[^A-Za-z\s]/g, "");
    specialization.value = sanitizedValue;
    event.target.value = sanitizedValue;
  }
};

const validateField = (field) => {
  errors.value[field] = "";
  switch (field) {
    case "medCred": {
      if (!medCred.value) {
        errors.value.medCred =
          "Please provide at least one medical credential.";
      }
      break;
    }
    case "specialization": {
      if(specialization.value){
        specialization.value = specialization.value.trim();
      }
      else if (!specialization.value) {
        errors.value.specialization = "Specialization field cannot be empty.";
      }
      break;
    }
  }
};

const handleSave = async () => {
  validateField("medCred");
  validateField("specialization");

  if (!docSign.value) {
    signatureError.value = "Please upload your signature";
    isDocSignError.value = true; // Show error for empty signature field
    return; // Stop form submission
  }

  const hasErrors =
    Object.values(errors.value).some((error) => error !== "");
  if (hasErrors) {
    return;
  }
  const payload = {
    Signature: docSign.value,
    MedicalCredentials: medCred.value,
    Specialization: specialization.value,
    UserId: userId.value,
  };
  try {
    const response = await axiosInstance.post("/api/settings/update-doctor", payload);
      if (response.data.success) {
        document.cookie = `specialization=${encodeURIComponent(specialization.value)}; path=/; Secure; SameSite=Strict`;

        window.dispatchEvent(new CustomEvent("settings-updated", {
          detail: {
            success: true,
            message: "Doctor's information updated successfully.",
            specialization: specialization.value
          }
        }));
        scrollToTop();
    } else {
      window.dispatchEvent(new CustomEvent("settings-updated", {
        detail: {
          success: false,
          message: "Doctor's information update failed."
        }
      }));
      scrollToTop();
    }
    message.value = response.data.message;
    success.value = response.data.success;
  } catch (error) {
    message.value = "An error occurred while updating the doctor's information.";
    success.value = false;

    window.dispatchEvent(
      new CustomEvent("toast-event", {
        detail: {
          success: success.value,
          message: message.value,
        },
      }));
    scrollToTop();
  } finally {
    scrollToTop();
  }
};

const scrollToTop = () => {
  window.scrollTo({
    top: 0,
    behavior: 'smooth'
  });
};
</script>

<style scoped>
.container{
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    gap: 150px;
    border-radius: 12px;
    background: #FFF;
} 

.docInfo-container{
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    gap: 18px;
}

.docInfo-title{
    color: #1C1C1C;
    font-size: 22px;
    font-weight: 600;
}

.fieldTexts{
    color: var(--Text-light, #5A5A5A);
    font-size: 14px;
    font-weight: 400;
}

.mandatorySymbol{
    color: #FD1414;
    font-size: 14px;
    font-weight: 700;
    line-height: 150%;
}

.signatureBox{
    width: 352.475px;
    height: 160px;
    flex-shrink: 0;
    border-radius: 3.168px;
    border: 0.792px dashed rgba(0, 102, 212, 0.30);
    background: #EEF6FF;
}

.hiddenFileInput {
    display: none;
}

.uploadIcon{
    margin-top: 30px;
    margin-left: 139px;
}

.dragAndDrop{
    padding: 3.96px;
    color: #0F0F0F;
    font-size: 12.673px;
    font-weight: 700;
    margin-left: 80px;
}

.browseText{
    color: #0066D4;
    font-size: 12.673px;
    font-weight: 700;
    line-height: 19.01px;
    text-decoration-line: underline;
    text-decoration-style: solid;
    text-decoration-skip-ink: none;
    text-decoration-thickness: auto;
    text-underline-offset: auto;
    text-underline-position: from-font;
    cursor: pointer;
}

.supportedText{
    padding: 3.96px;
    color: #676767;
    font-size: 9.505px;
    font-weight: 400;
    line-height: 14.257px;
    margin-left: 92px;
}

.imagePreview {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100%;
}

.previewImage {
    border-radius: 3px;
    width: 352.475px;
    height: 160px;
    object-fit: fill;
}

.delete-icon {
  position: absolute;
  left: 788px;
  top: 242px;
  cursor: pointer;
}

.trashIcon{
  width: 89px;
  height: 20px;
  margin-top: 3px;
}

.inputTextBoxes{
    width: 332.48px;
    height: 25px;
    border-radius: 4px;
    border: 1px solid #EEE;
    padding: 10px;
    outline: none;
}

.nameField{
  padding-top: 10px;
}

.medicalCredSpecGroup{
    display: flex;
    align-items: flex-start;
    gap: 30px;
}

.medCredDropDownBox{
    width: 354.08px;
    height: 46.6px;
    border-radius: 4px;
    border: 1px solid #EEE;
    padding: 10px;
    outline: none;
}

.btnSession {
  display: flex;
  justify-content: space-between;
  width: 100%;
  margin-top: 80px;
  gap: 830px;
}

.cancel-btn {
  display: flex;
  height: 42px;
  padding: 8px 20px;
  justify-content: center;
  align-items: center;
  gap: 8px;
  flex-shrink: 0;
  border-radius: 8px;
  border: 1px solid #eee;
  background: var(--bg-white, #fff);
  cursor: pointer;
}

.save-btn {
  display: flex;
  height: 42px;
  padding: 8px 20px;
  justify-content: center;
  align-items: center;
  gap: 8px;
  flex-shrink: 0;
  border-radius: 8px;
  background: var(--Primary-Colors, #0066d4);
  cursor: pointer;
  border: none;
  color: var(--Text-Text-white, #fff);
}

.error {
  color: red;
  font-size: 12px;
  position: absolute;
}

.error-image-upload{
  color: red;
  font-size: 12px;
  position: absolute;
}

</style>