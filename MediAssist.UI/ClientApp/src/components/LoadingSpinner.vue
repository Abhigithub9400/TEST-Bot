<!-- Loader.vue -->
<template>
  <div v-if="visible" class="loader-overlay">
    <div class="progress-loader">
      <svg class="circle" viewBox="0 0 36 36">
        <path class="circle-bg" d="M18 2a16 16 0 1 1 0 32 16 16 0 0 1 0-32"></path>
        <path class="circle-progress" d="M18 2a16 16 0 1 1 0 32 16 16 0 0 1 0-32"></path>
      </svg>
    </div>
    <p>{{ message || 'Loading ...' }}</p>
  </div>
</template>

  <script>
  export default {
    props: {
      visible: {
        type: Boolean,
        default: false,
      },
      message: {
        type: String,
        default: "",
      }
    },
  };
  </script>
  
  <style scoped>
  .loader-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.progress-loader {
  width: 100px;
  height: 100px;
}

svg.circle {
  transform: rotate(-90deg);
  width: 100%;
  height: 100%;
}

.circle-bg {
  fill: none;
  stroke: #ddd;
  stroke-width: 3.8;
}

.circle-progress {
  fill: none;
  stroke: #3498db;
  stroke-width: 3.8;
  stroke-linecap: round;
  stroke-dasharray: 100, 100; 
  stroke-dashoffset: 100; 
  animation: progress 2s linear infinite;
}

@keyframes progress {
  0% {
    stroke-dashoffset: 100;
  }
  100% {
    stroke-dashoffset: 0;
  }
}

p {
  margin-top: 20px;
  color: #fff;
  font-size: 16px;
  text-align: center;
}
</style>
  