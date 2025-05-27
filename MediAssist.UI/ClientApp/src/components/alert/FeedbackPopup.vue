<template>
  <div v-if="isVisible" class="modal-overlay">
    <div class="modal">
      <div class="modal-header">
        <h2 class="modal-title">{{ props.title }}</h2>
        <button class="close-button" @click="cancel">
          <span>&times;</span>
        </button>
      </div>
      
      <div class="modal-body">
      <span v-if="hasSubmitAttempt && errors" class="error-message">{{
        errors
      }}</span>
        <div class="star-rating">
          <span aria-required="true"
            v-for="star in 5"
            :key="star"
            @click="setRating(star)"
            :class="{
              star: true,
            }"
          >
            <img
              v-if="rating >= star"
              src="@/assets/filledStar.jpg"
              alt="Star Icon"
            />
            <img v-else src="@/assets/star.png" alt="Unfilled Star Icon" />
          </span>
        </div>

        <div class="category">Feedback Categories <span class="required">*</span></div>
        <div class="dropdown-container" ref="dropdownRef">
          <!-- Selected items display -->
          <div class="selected-items" @click="toggleDropdown">
            <div v-if="selectedItems.length === 0" class="placeholder-wrapper">
              <span class="placeholder">Select</span>  
              <img class="arrow-down" src="@/assets/alt-arrow-down.png" alt="Arrow Down Icon" />
            </div>
            <div v-else class="selected-items-wrapper">
              <div
                v-for="item in selectedItems"
                :key="item.id"
                class="selected-tag"
                aria-required="true"
              >
                <div class="frame">
                  <div class="text-wrapper">{{ item.label }}</div>
                  <button class="remove-button" @click="removeItem(item.id)">
                    <img
                      class="vector"
                      alt="Vector"
                      src="@/assets/close-icon.png"
                    />
                  </button>
                </div>
              </div>
            </div>
          </div>
          <div v-if="isOpen" class="dropdown-card">
            <div
              v-for="option in options"
              :key="option.id"
              class="dropdown-item"
              :class="{ selected: isSelected(option.id) }"
              @click="toggleItem(option)"
            >
              <span>{{ option.label }}</span>
              <span v-if="isSelected(option.id)" class="checkmark">
                <img src="@/assets/tick-icon.png" alt="Tick Icon" />
              </span>
            </div>
          </div>
        </div>
        <Transition name="fade">
          <div v-if="isOtherCategory" class="other-category-container">
            <input
              v-model="otherCategoryText"
              type="text"
              class="other-input"
              placeholder="Please specify"
              @input="updateOtherCategory"
            />
          </div>
        </Transition>
        <div class="description">Description of Issue<span class="required">*</span></div>
        <div class="textarea-container">
          <textarea
            v-model="description"
            maxlength="100"
            class="textarea"
            required
            @input="validateIfSubmitted"
          ></textarea>
          <small class="small-text">{{ description.length }}/100</small>
        </div>
        <div class="suggestion">Suggestions for Improvement</div>
        <div class="textarea-container">
          <textarea
            v-model="suggestion"
            maxlength="100"
            class="textarea"
          ></textarea>
          <small class="small-text">{{ suggestion.length }}/100</small>
        </div>
      </div>
      <div class="modal-footer">
        <div class="modal-buttons">
          <button @click="confirm" class="confirm-button">
            {{ confirmText }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, defineProps, defineEmits, onMounted, onUnmounted, watch } from "vue";

const props = defineProps({
  isVisible: {
    type: Boolean,
    required: true,
  },
  title: {
    type: String,
    default: "Confirmation",
  },
  confirmText: {
    type: String,
    default: "Confirm",
  },
  cancelText: {
    type: String,
    default: "Cancel",
  },
  options: {
    type: Array,
    default: () => [
      { id: 1, label: "Usability" },
      { id: 2, label: "Performance" },
      { id: 3, label: "Features" },
      { id: 4, label: "Bugs" },
      { id: 5, label: "Other" },
    ],
  },
});

const emit = defineEmits([
  "confirm",
  "cancel",
  "setRating",
  "update:selectedItems",
]);

const rating = ref(0);
const isOpen = ref(false);
const selectedItems = ref([]);
const dropdownRef = ref(null);
const otherCategoryText = ref("");
const description = ref("");
const suggestion = ref("");
const errors = ref("");
const hasSubmitAttempt = ref(false);

const validateIfSubmitted = () => {
  if (hasSubmitAttempt.value) {
    validateForm();
  }
};

watch([rating, selectedItems, description], () => {
  validateIfSubmitted();
}, { deep: true });

const validateForm = () => {
  const validations = {
    rating: rating.value !== 0,
    categories: selectedItems.value.length > 0,
    description: description.value.trim().length > 0
  };

  if (!validations.rating || !validations.categories || !validations.description) {
    errors.value = "Please complete all required fields before submitting.";
  } else {
    errors.value = "";
  }

  return !errors.value;
};

const updateOtherCategory = () => {
  emit("update:selectedItems", selectedItems.value);
  validateIfSubmitted();
};

const isOtherCategory = ref(false);

const toggleDropdown = () => {
  isOpen.value = !isOpen.value;
};

const isSelected = (id) => {
  return selectedItems.value.some((item) => item.id === id);
};

const toggleItem = (option) => {
  isOtherCategory.value = option.id === 5;
  const index = selectedItems.value.findIndex((item) => item.id === option.id);
  if (index === -1) {
    selectedItems.value.push(option);
  } else {
    selectedItems.value.splice(index, 1);
  }
  validateIfSubmitted();
};

const removeItem = (id) => {
  selectedItems.value = selectedItems.value.filter((item) => item.id !== id);
  if (id === 5) {
    otherCategoryText.value = "";
    isOtherCategory.value = false;
  }
  validateIfSubmitted();
};

const handleClickOutside = (event) => {
  if (dropdownRef.value && !dropdownRef.value.contains(event.target)) {
    isOpen.value = false;
  }
};

onMounted(() => {
  document.addEventListener("click", handleClickOutside);
});

onUnmounted(() => {
  document.removeEventListener("click", handleClickOutside);
});

const setRating = (value) => {
  if (rating.value === value) {
    rating.value = 0;
  } else {
    rating.value = value;
  }
  validateIfSubmitted();
  emit("setRating", rating.value);
};

const confirm = () => {
  hasSubmitAttempt.value = true;
  if (validateForm()) {
    emit("update:selectedItems", selectedItems.value);
    emit("text:otherCategoryText", otherCategoryText.value);
    emit("confirm", {
      rating: rating.value,
      selectedItems: selectedItems.value,
      description: description.value,
      suggestion: suggestion.value,
      otherCategoryText: otherCategoryText.value
    });
    rating.value = 0;
    selectedItems.value = [];
    description.value = "";
    suggestion.value = "";
    otherCategoryText.value = "";
    isOtherCategory.value = false;
    hasSubmitAttempt.value = false;
    errors.value = "";
  }
};

const cancel = () => {
  emit("cancel");
  rating.value = 0;
    selectedItems.value = [];
    description.value = "";
    suggestion.value = "";
    otherCategoryText.value = "";
    isOtherCategory.value = false;
    hasSubmitAttempt.value = false;
    errors.value = "";
};
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: flex-start;
  z-index: 1000;
}

.modal {
  display: flex;
  top: 30px;
  align-items: center;
  padding: 25px 40px 40px 40px;
  gap: 1px;
  border-radius: 17px;
  background: #ffffff;
  flex-direction: column;
  position: absolute;
  width: 410px;
  height: 600px;
  justify-content: space-between;
}

.close-button {
  width: 32px;
  height: 32px;
  padding: 8px;
  top: 15px;
  position: absolute;
  right: 24px;
  font-size: 35px;
  cursor: pointer;
  color: #aaa;
  border: none;
  background: none;
}

.close-button:hover {
  color: black;
}

.modal-header {
  display: flex;
  width: 100%;
  justify-content: space-between;
}

.modal-title {
  width: 280px;
  height: 28px;
  font-style: normal;
  margin-top: 10px;
}

.modal-buttons {
  display: flex;
  gap: 30px;
}

.modal-body {
  display: flex;
  flex-direction: column;
  height: auto;
  overflow-y: scroll;
  scrollbar-width: none;
  margin-top: 4px;
}

.modal-footer {
  display: flex;
}

.confirm-button {
  cursor: pointer;
  width: 45%;
  border-radius: 8px;
  border: 1px solid #d9d9d9;
  display: flex;
  height: 48px;
  padding: 8px 20px;
  justify-content: center;
  align-items: center;
  gap: 8px;
  flex: 1 0 0;
  margin-top: 16px;
  background-color: #0066cc;
  color: #fff;
  font-size: 16px;
  font-style: normal;
  font-weight: 600;
  line-height: 150%;
}

.confirm-button:hover {
  background-color: #005bb5;
}

.star-rating {
  display: flex;
  width: 100%;
  gap: 18.271px;
  align-items: center;
  flex-direction: row;
}

.star.filled {
  color: #ffd700;
}

.star {
  font-size: 24px;
  color: #ddd;
  cursor: pointer;
  transition: color 0.2s;
}

.category {
  color: #1c1c1c;
  font-size: 16px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%;
  margin-bottom: 12px;
  margin-top: 0;
}

.dropdown-container {
  border-radius: 5px;
  border: 1px solid #eee;
}

.placeholder-wrapper {
  display: flex;
  width: 100%;
  justify-content: space-between;
  align-items: center;
  padding: 8px;
}

.placeholder {
  color: #bbb;
  font-size: 13.364px;
  font-style: normal;
  font-weight: 400;
  line-height: normal;
}

.arrow-down {
  width: 26.727px;
  height: 22.909px;
  flex-shrink: 0;
}

.selected-items {
  border-radius: 5px;
  display: flex;
  width: 396px;
  padding: 5px 10px;
  align-items: center;
}

.selected-items-wrapper {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  min-height: 24px;
  width: 100%;
}

.dropdown-item {
  padding: 8px 16px;
  cursor: pointer;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.dropdown-item:hover {
  background-color: #f5f5f5;
}

.dropdown-item.selected {
  background-color: #f8f8f8;
}

.dropdown-card {
  position: absolute;
  top: 203px;
  left: 39px;
  right: 38px;
  margin-top: 8px;
  background: white;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  z-index: 1000;
  overflow: hidden;
}

.frame {
  align-items: center;
  background-color: #f7f9fc;
  border: 1px solid #e3e9f1;
  display: inline-flex;
  gap: 5px;
  height: 30px;
  position: relative;
  border-radius: 4px;
  padding: 0 8px;
}

.text-wrapper {
  color: #333333;
  font-size: 12px;
  font-weight: 400;
  letter-spacing: 0;
  line-height: normal;
  margin-bottom: -1.5px;
  margin-top: -3.5px;
  position: relative;
  width: fit-content;
}

.vector {
  height: 10px;
  position: relative;
  width: 10px;
}

.remove-button {
  border: none;
  background: none;
  cursor: pointer;
  padding: 0;
  display: flex;
  align-items: center;
}

.checkmark {
  fill: #ccff90;
  width: 10.333px;
  height: 8px;
  flex-shrink: 0;
}

.description {
  color: #1c1c1c;
  font-size: 16px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%;
  margin-bottom: 16px;
  margin-top: 17px;
}
.textarea-container {
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
  align-items: flex-start;
  flex: 1 0 0;
  align-self: stretch;
}
.textarea {
  border-radius: 6px;
  border: 1px solid var(--Colors-Border-Colors-Border-Color, #e3e9f1);
  background: var(--bg-white, #fff);
  display: flex;
  width: 404px;
  height: 101px;
  padding-top: 8px;
  flex-direction: column;
  align-items: flex-start;
  gap: 8px;
  flex-shrink: 0;
  resize: none;
}

.textarea:focus {
  outline: none;
  border: 1px solid #eee;
}

.small-text {
  color: var(--Colors-Text-Colors-Text-Label, #6d6d6d);
  text-align: right;
  margin-left: -11px;
  width: 100%;
  font-size: 10px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%;
}

.suggestion {
  color: #1c1c1c;
  font-size: 16px;
  font-style: normal;
  font-weight: 400;
  line-height: 150%;
  margin-top: 15px;
  margin-bottom: 15px;
}

.selected-tag {
  color: #333;
  font-size: 12px;
  font-style: normal;
  font-weight: 400;
  line-height: normal;
}

.other-category-container {
  margin: 8px 0;
  display: flex;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease, transform 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}

.other-input {
  width: 100%;
  padding: 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
  transition: all 0.2s ease;
}

.other-input:focus {
  outline: none;
  border: 1px solid #ddd;
  box-shadow: 0 0 0 2px rgba(76, 175, 80, 0.1);
}

.error-message {
  color: #ff4444;
  font-size: 12px; 
  display: block;
  margin-bottom: 21px;
}
.required {
        color: #FD1414;
        font-size: 16px;
        font-style: normal;
        font-weight: 700;
        line-height: 150%;
}
@media (max-width: 575px) { 
  .modal{
    width: 80%;
    padding: 25px;
    }
 .modal-body{
   width: 100%;
 }
 .textarea{
  width: 98%;
 }
}

</style>
