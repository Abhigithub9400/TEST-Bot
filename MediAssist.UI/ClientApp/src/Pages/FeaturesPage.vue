<template>
  <div class="features" v-scroll-reveal>
    <section class="hero-section">
      <HeroSection
        title="Advanced AI Features for Efficient Medical Transcription"
        :description="`Empower your practice with cutting-edge AI technology that enhances documentation efficiency, reduces administrative workload, and improves clinical accuracy. Discover how ${store.MediAssistConfigManager.DomainName} transforms medical transcription:`"
        :imageUrl="require('@/assets/features-hero-image.jpg')"
        buttonText="Book a Demo"
        buttonLink="https://example.com/demo"
      />
    </section>
    <section class="features-list-section" id="featureCards" ref="featureCards">
      <FeaturesListComponent></FeaturesListComponent>
    </section>
    <section class="accordian-secion">
      <AccordianComponent :tabs="tabs" />
    </section>
    <section class="book-demo-section" id="bookDemo" ref="bookDemo">
      <BookDemoComponent></BookDemoComponent>
    </section>
  </div>
</template>
<script setup>
import { ref, watch, onMounted } from "vue";
import BookDemoComponent from "@/components/BookDemoComponent.vue";
import FeaturesListComponent from "@/components/FeaturesListComponent.vue";
import HeroSection from "@/components/HeroSection.vue";
import { useMyStore } from "@/store/store.ts";
import AccordianComponent from "@/components/AccordianComponent.vue";

const store = useMyStore();
const featureCards = ref(null);
const bookDemo = ref(null);
const tabs = ref([]);

const initializePageContent = () => {
  const domain = store.MediAssistConfigManager?.DomainName;
  tabs.value = [
    {
      title: `What makes ${domain} the best medical transcription software for physicians?`,
      content: `${domain} offers real-time, AI-powered transcription, automatic SOAP note generation, and seamless EHR integration-making it one of the best medical transcription software options for physicians aiming to reduce documentation time.`,
      value: '0',
    },
    {
      title: `How does ${domain} integrate with existing EHR systems?`,
      content: `${domain} is a medical dictation software with EHR integration that enables easy transfer of structured clinical notes into your preferred EHR system, reducing manual data entry and improving accuracy.`,
      value: '1',
    },
    {
      title: `Is ${domain} suitable for clinics and hospitals of all sizes?`,
      content: `Yes, whether you're a solo practitioner or a large healthcare organization, ${domain} is a scalable medical transcription software designed to meet diverse documentation needs efficiently.`,
      value: '2',
    },
    {
      title: `How accurate is the medical dictation software in transcribing conversations?`,
      content: `${domain} uses advanced AI and NLP to deliver high transcription accuracy, understanding medical terminology and context to create precise clinical documentation in real time.`,
      value: '3',
    },
    {
      title: `Can ${domain} generate SOAP notes automatically?`,
      content: `Absolutely. ${domain} converts real-time conversations into structured SOAP notes, helping physicians save time and focus more on patient care rather than paperwork.`,
      value: '4',
    },
  ];
};

onMounted(() => {
  initializePageContent();
});

watch(
  () => store.MediAssistConfigManager?.DomainName,
  (newVal) => {
    if (newVal) {
      initializePageContent();
    }
  },
  { immediate: true }
);
</script>



<style>
.features {
  background-color: #ffffff;
  height: 100%;
  overflow: hidden;
  width: 100%;
  gap: 20px;
}

.hero-section {
  width: 100%;
}

.features-list-section {
  width: 1240px;
  margin-inline: auto;
  margin-top: 5rem;
}

.accordian-secion {
  margin-top: 60px;
  height: 803px;
  background-color: #f7fbff;
}
</style>
