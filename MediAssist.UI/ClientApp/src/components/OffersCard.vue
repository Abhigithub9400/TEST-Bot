<template>
  <div class="card">
    <img :src="props.icon" :alt="props.title" class="card-image" />
    <div class="card-content">
      <h2 class="card-title">{{ title }}</h2>
      <p class="card-description">
        {{ description }}
      </p>
    </div>
  </div>
</template>

<script setup>
import { defineProps } from "vue";

const props = defineProps({
  icon: String,
  title: String,
  description: String,
});
</script>

<style scoped>
.card {
  width: 301px;
  height: 261px;
  border-radius: 10px;
  box-shadow: 0px 4px 14px -1px #e5f1ff;
  position: relative;
  overflow: hidden;
  cursor: pointer;
}
.card:hover{
  transform: translateY(-5px);
}
.card-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.card-content {
  position: absolute;
  bottom: 0;
  left: 0;
  padding: 14px;
  background: rgba(255, 255, 255, 255);
  width: 92%;
  box-sizing: border-box;
  border-radius: 0px 10px;
  height: 107px;
}

.card-title {
  font-size: 0.9rem;
  font-weight: 600;
  margin: 0;
  color: #1c1c1c;
}

.card-description {
  font-size: 0.9rem;
  margin: 6px 0 0;
  color: #555;
  line-height: 130%
}
</style>
