<template>
    <Transition name="fade">
      <div v-if="isVisible" :class="['alert', type]">
        <div class="frame">
          <img
            class="icon"
            :alt="iconAlt"
            :src="iconSrc"
          />
          <div class="text-wrapper">{{ message }}</div>
          <img class="close-icon" alt="Close icon" :src="closeIcon" @click="emitClose" />
        </div>
      </div>
    </Transition>
  </template>
  
  <script setup>
  import { computed, defineProps, defineEmits } from 'vue';
  import successIcon from "@/assets/icons/check-circle.png";
  import errorIcon from "@/assets/icons/error-circle.png";
  import closeIcon from "@/assets/icons/close-icon.png";
  import infoIcon from "@/assets/icons/info-icon.png";
  
    const props = defineProps({
    message: {
        type: String,
        required: true
    },
    type: {
        type: String,
        default: 'success',
        validator: (value) => ['success', 'error', 'info'].includes(value)
    },
    isVisible: {
        type: Boolean,
        default: false
    }
    });
  
  const emit = defineEmits(['close']);
  
  const iconSrc = computed(() => {
    switch (props.type) {
      case 'success':
        return successIcon;
      case 'error':
        return errorIcon;
      case 'info':
        return infoIcon;
      default:
        return successIcon;
    }
  });
  
  const iconAlt = computed(() => `${props.type.charAt(0).toUpperCase() + props.type.slice(1)} icon`);
  
  const emitClose = () => {
    emit('close');
  };
  </script>
   
  <style scoped>
  .alert {
    align-items: flex-start;
    border-radius: 4px;
    display: inline-flex;
    flex-direction: column;
    gap: 10px;
    padding: 10px;
    position: relative;
    max-width: 780px; 
  }
  
  .alert.success {
    background-color: #EDF8ED;
  }
  
  .alert.error {
    background-color: #FFDFDD;
  }
  
  .alert.info {
    background-color: #E5F1FB;
  }
  
  .alert .frame {
    align-items: center;
    display: flex;
    flex: 1;
    gap: 8px;
    position: relative;
    width: 100%;
  }
  
  .alert .icon {
    height: 21px;
    position: relative;
    width: 21px;
    flex-shrink: 0;
  }
  
  .alert .text-wrapper {
    color: var(--colors-text-colors-text-primary);
    font-size: 14px;
    font-weight: 400;
    letter-spacing: 0;
    line-height: 21px;
    position: relative;
    white-space: normal;
    overflow-wrap: break-word;
    flex: 1;
    max-height: 42px;
    overflow: hidden;
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
  }
  
  .alert .close-icon {
    height: 9.7px;
    position: relative;
    width: 10.2px;
    cursor: pointer;
    flex-shrink: 0;
  }
  
  .fade-enter-active, .fade-leave-active {
    transition: opacity 0.5s;
  }
  .fade-enter-from, .fade-leave-to {
    opacity: 0;
  }
  </style>