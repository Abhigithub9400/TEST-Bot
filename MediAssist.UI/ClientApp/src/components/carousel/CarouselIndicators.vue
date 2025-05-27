<script setup>
import { defineEmits, defineProps } from 'vue';

defineEmits(['switch']);
defineProps(['total', 'currentIndex', 'screenSize']);
</script>

<template>
  <div class="carousel-indicators" :class="screenSize">
    <button
        class="carousel-indicator-item"
        :class="{ active: currentIndex === index }"
        v-for="(item, index) in total"
        :key="index"
        @click="$emit('switch', index)"
    ></button>
  </div>
</template>

<style scoped>
.carousel-indicators {
  position: absolute;
  transform: translateX(-50%);
  left: 50%;
  bottom: 0;
  z-index: 2;
}
.carousel-indicator-item {
  width: 0.75rem;
  height: 0.75rem;
  border: none;
  background: rgba(217, 217, 217, 0.60);
  opacity: 0.5;
  margin: 0.2em;
  border-radius: 50%;
  cursor: pointer;
}

.active {
  opacity: 1;
  background: #1949BF;
}
</style>