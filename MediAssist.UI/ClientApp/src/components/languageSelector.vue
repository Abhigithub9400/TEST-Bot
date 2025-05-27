<template>
    <div class="language-dropdown" ref="dropdownRef">
        <button @click="toggleDropdown" class="dropdown-button">
            <img :src="recognisationlanguageicon" alt="language icon" class="language-icon" />
            <div class="selected-language-label">{{ selectedLanguage.label }}</div>
            <span class="arrow-icon">
                <svg v-if="isOpen" width="12" height="6" viewBox="0 0 12 6" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path fill-rule="evenodd" clip-rule="evenodd" d="M11.8602 5.79046C11.6515 6.04206 11.2849 6.0712 11.0415 5.85554L6 1.39024L0.958532 5.85554C0.715054 6.0712 0.348493 6.04206 0.139797 5.79046C-0.0688992 5.53887 -0.0407019 5.16009 0.202777 4.94444L5.62212 0.144446C5.83957 -0.0481481 6.16043 -0.0481481 6.37788 0.144446L11.7972 4.94444C12.0407 5.16009 12.0689 5.53887 11.8602 5.79046Z" fill="#959595" />
                </svg>
                <svg v-else width="12" height="6" viewBox="0 0 16 8" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path fill-rule="evenodd" clip-rule="evenodd" d="M0.186396 0.279381C0.464658 -0.0560792 0.953405 -0.0949283 1.27804 0.192609L8 6.14634L14.722 0.192609C15.0466 -0.0949277 15.5353 -0.0560785 15.8136 0.279382C16.0919 0.614842 16.0543 1.11988 15.7296 1.40742L8.50384 7.80741C8.21391 8.0642 7.78609 8.0642 7.49616 7.80741L0.270369 1.40742C-0.0542702 1.11988 -0.0918661 0.614841 0.186396 0.279381Z" fill="#959595" />
                </svg>
            </span>
        </button>
        <div v-if="isOpen" class="dropdown-menu">
            <div v-for="(language, index) in languages" :key="index" class="dropdown-item">
                <input type="radio"
                       :id="`lang-${index}`"
                       :value="language"
                       v-model="selectedLanguage"
                       @change="handleLanguageChange(language)" />
                <label :for="`lang-${index}`">{{ language.label }}</label>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, watch, defineEmits, onMounted, onBeforeUnmount } from 'vue';
import recognisationlanguageicon from "@/assets/icons/language-selector-icon.png";

const isOpen = ref(false);
const dropdownRef = ref(null);

const languages = [
  {
    label: 'English',
    code: 'en-US'
  },{
    label: 'عربى',
    code: 'ar-SA'
  },
  {
    label: 'മലയാളം',
    code: 'ml-IN'
  },
  {
    label: 'हिन्दी',
    code: 'hi-IN'
  }
];

const selectedLanguage = ref(languages[0]);

const emit = defineEmits('languageChange');

const toggleDropdown = () => {
  isOpen.value = !isOpen.value;
};

const handleLanguageChange = (language) => {
  emit('languageChange', language.code);
  isOpen.value = false;
};

const handleClickOutside = (event) => {
  if (dropdownRef.value && !dropdownRef.value.contains(event.target)) {
    isOpen.value = false;
  }
};

onMounted(() => {
  document.addEventListener('click', handleClickOutside);
});

onBeforeUnmount(() => {
  document.removeEventListener('click', handleClickOutside);
});

watch(selectedLanguage, () => {
  isOpen.value = false;
});
</script>

<style scoped>
    .language-dropdown {
        position: relative;
        display: inline-block;
    }

    .dropdown-button {
        display: flex;
        align-items: center;
        justify-content: space-between;
        width: 190px;
        padding: 8px;
        background-color: #fff;
        border: 1px solid #EEEEEE;
        border-radius: 4px;
        cursor: pointer;
    }

    .language-icon {
        width: 26px;
        height: 20px;
        margin-right: 8px;
    }

    .selected-language-label {
        width: 107px;
        color: #141414;
        font-family: Inter, sans-serif;
        font-size: 12px;
        font-weight: 400;
        line-height: 21px;
        text-align: left;
    }

    .arrow-icon {
        color: #959595;
        padding-left: 5px;
        transition: transform 0.2s ease;
    }

    .dropdown-menu {
        position: absolute;
        width: 190px;
        height: auto;
        top: 100%;
        left: 0;
        margin-top: 4px;
        background-color: white;
        border: 1px solid #EEEEEE;
        border-radius: 10px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        z-index: 1000;
        overflow: hidden;
    }

    .dropdown-item {
        display: flex;
        align-items: center;
        width: 100%;
        padding: 8px 16px;
        cursor: pointer;
        border-bottom: 1px solid #EEEEEE;
        position: relative;
    }

        .dropdown-item:last-child {
            border-bottom: none;
        }

        .dropdown-item:hover {
            background-color: #f0f0f0;
        }

        .dropdown-item input[type="radio"] {
            margin-right: 8px;
        }

        .dropdown-item label {
            font-family: Inter, sans-serif;
            font-size: 12px;
            font-weight: 400;
            line-height: 21px;
            text-align: left;
        }
</style>