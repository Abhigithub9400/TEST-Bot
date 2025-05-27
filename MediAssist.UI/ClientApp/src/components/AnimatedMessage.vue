<template>
    <span class="message-text">
      {{ displayedText }}
      <span v-if="!isComplete" class="typing-cursor"></span>
    </span>
  </template>
  
  <script setup>
  import { ref, watch, onMounted, onUnmounted , defineProps} from 'vue';
  
  const props = defineProps({
    text: {
      type: String,
      required: true
    },
    speed: {
      type: Number,
      default: 30
    }
  });
  
  const displayedText = ref('');
  const isComplete = ref(false);
  let intervalId = null;
  
  const animateText = () => {
    let currentIndex = 0;
    displayedText.value = '';
    isComplete.value = false;
  
    intervalId = setInterval(() => {
      if (currentIndex < props.text.length) {
        displayedText.value += props.text[currentIndex];
        currentIndex++;
      } else {
        isComplete.value = true;
        clearInterval(intervalId);
      }
    }, props.speed);
  };
  
  watch(() => props.text, () => {
    if (intervalId) clearInterval(intervalId);
    animateText();
  });
  
  onMounted(() => {
    animateText();
  });
  
  onUnmounted(() => {
    if (intervalId) clearInterval(intervalId);
  });
  </script>