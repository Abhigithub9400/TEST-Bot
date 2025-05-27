<template>
  <div class="hero-header" v-scroll-reveal>
    <div class="overlap">
      <div class="rectangle" />
      <div class="content-container">
        <div class="frame">
          <div class="frame-wrapper">
            <div class="div">
              <h1 class="text-wrapper h1-semibold">
                {{ title }}
              </h1>
              <p class="p lg-regular">
                {{ description }}
              </p>
            </div>
          </div>
          <button v-if="buttonText && buttonLink" class="hero-button btn btn-primary" @click="handleRequestDemo('request-demo')">
            <div class="btn-background">
              <div class="inside-text">
                <span>{{ buttonText }}</span>
              </div>
            </div>
            <div class="border-gradient-fn"></div>
          </button>
        </div>
        <img class="rectangle-2" alt="Rectangle" :src="imageUrl" />
      </div>
    </div>
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router';
import { defineProps } from "vue";

const router = useRouter();

defineProps({
  title: { type: String, required: true },
  description: { type: String, required: true },
  imageUrl: { type: String, required: true },
  buttonText: { type: String, default: "" },
  buttonLink: { type: String, default: "" },
});

const handleRequestDemo = (modalType) => {
  localStorage.setItem('modalType', modalType);
  router.push('/request-demo');
};
</script>


<style>
.hero-header {
  background-color: #ffffff;
  height: 662px;
  overflow: hidden;
  width: 100%;
  margin-inline: auto;
}

.hero-header .overlap {
  height: 662px;
  position: relative;
}

.hero-header .rectangle {
  background: linear-gradient(0deg, rgba(5, 6, 19, 1) 0%, rgba(5, 27, 47, 1) 100%);
  height: 661px;
  position: absolute;
  top: 0;
  width: 100%;
}

.hero-header .content-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 1277px;
  margin-inline: auto;
  position: relative;
  top: 158px;
}

.hero-header .frame {
  display: flex;
  flex-direction: column;
  gap: 32px;
  width: 50%;
}

.hero-header .text-wrapper {
  color: #ffffff;
  margin: 20px 0;
}

.hero-header .p {
  color: #ffffff;
  margin: 0;
}

.hero-header .hero-button {
  width: 130px;
}

.hero-header .rectangle-2 {
  height: 503px;
  width: 503px;
  border-radius: 24px 24px 0px 0px;
}
</style>
