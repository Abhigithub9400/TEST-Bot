<template>
  <div class="box">
    <div class="rectangle">
      <transition name="slide-up">
        <div class="label" :key="currentFeature.featureTitle">
          <div class="text-wrapper">{{ currentFeature.featureTitle }}</div>
        </div>
      </transition>
    </div>

    <transition name="slide-up">
      <div class="image-section" :key="currentFeature.featureImage">
        <img :src="currentFeature.featureImage" :alt="currentFeature.featureTitle" class="background-image" />
      </div>
    </transition>
  </div>
</template>


<script>
export default {
  name: "SlidingFeatureCard",
  data() {
    return {
      features: [
        {
          featureTitle: "Real-Time Transcription",
          featureImage: require("@/assets/HomePageImages/SlidingFeatureCard/real-time-transcript.jpg"),
        },
        {
          featureTitle: "Potential Diagnosis Feature Control",
          featureImage: require("@/assets/HomePageImages/SlidingFeatureCard/potential-diagnosis-with-feature-control.jpg"),
        },
        {
          featureTitle: "Automated Prescription Generation",
          featureImage: require("@/assets/HomePageImages/SlidingFeatureCard/automated-prescription.jpg"),
        },
        {
          featureTitle: "Comprehensive Report Creation",
          featureImage: require("@/assets/HomePageImages/SlidingFeatureCard/report-generation.jpg"),
        },
      ],
      currentIndex: 0,
      intervalId: null,
    };
  },
  computed: {
    currentFeature() {
      return this.features[this.currentIndex];
    },
  },
  mounted() {
    this.preloadImages();
    this.startAutoSlide();
  },
  beforeUnmount() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  },
  methods: {
    startAutoSlide() {
      this.intervalId = setInterval(() => {
        this.currentIndex = (this.currentIndex + 1) % this.features.length;
      }, 5000);
    },
    preloadImages() {
      this.features.forEach((feature) => {
        const img = new Image();
        img.src = feature.featureImage;
      });
    },
  },
};
</script>

<style scoped>
.box {
  height: 604px;
  width: 607px;
  position: relative;
  overflow: hidden;
}

.rectangle {
  background: linear-gradient(112deg, rgba(78, 168, 227, 1) 0%, rgba(34, 48, 107, 1) 100%);
  border-radius: 24px;
  height: 604px;
  width: 607px;
  position: absolute;
  display: flex;
  align-items: flex-start;
  justify-content: center;
  padding-top: 50px;
  overflow: hidden;
}

.text-wrapper {
  color: #fff;
  font-size: 22px;
  font-weight: 600;
  text-align: center;
  line-height: 1.3;
}

.image-section {
  background-color: #ffffff;
  border: 1px solid #e9f3ff;
  border-radius: 33px 33px 0 0;
  height: 478px;
  width: 519px;
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  margin-inline: auto;
  overflow: hidden;
}

.background-image {
  position: absolute;
  inset: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.slide-up-enter-active {
  transition: all 0.8s ease-in-out;
}

.slide-up-enter-from {
  opacity: 0;
  transform: translateY(100%);
}

.slide-up-enter-to {
  opacity: 1;
  transform: translateY(0);
}

.slide-up-leave-from {
  opacity: 1;
}

.slide-up-leave-to {
  opacity: 0;
}
</style>
