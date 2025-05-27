<script setup>
import ScheduleDemoPopUp from "@/Pages/modals/GenericModal.vue";
import { onMounted, onUnmounted, ref, watch } from "vue";
import { useRouter } from "vue-router";
import { useMyStore } from "@/store/store.ts";
import BenifitsCard from "@/components/BenefitsCard.vue";
import AccordianComponent from "@/components/AccordianComponent.vue";
import SlidingFeatureCard from "@/components/carousel/SlidingFeatureCard.vue";
import BookDemoComponent from "@/components/BookDemoComponent.vue";

const router = useRouter();
const store = useMyStore();
const hero = ref(null);
const heroImage = ref(null);
const heroInfo = ref(null);
const contentWrapper = ref(null);
const heroVideo = ref(null);
const isScrolled = ref(false);
const screenSize = ref("large");
const showScheduleDemoPopUp = ref(false);
let translateLimit = 0;
let lastScrollPosition = 0;
let isFrozen = false;
let resizeObserver = null;
const tabs = ref([]);

const speciality = ref(null);
const highlights = ref(null);
const howItWorks = ref(null);
const featureCards = ref(null);
const bookDemo = ref(null);
const accordian = ref(null);

const updateScreenSize = () => {
  if (window.matchMedia("(max-width: 480px)").matches) {
    screenSize.value = 'small';
  } else if(window.matchMedia("(min-width: 481px) and (max-width: 768px)").matches) {
    screenSize.value = 'medium';
  } else if (window.matchMedia("(min-width: 769px) and (max-width: 1440px)").matches) {
    screenSize.value = 'large';
  } else if (window.matchMedia("(min-width: 1570px) and (max-width: 4615px)").matches) {
    if (window.matchMedia("(min-width: 1728px) and (max-width: 5098px)").matches) {
      screenSize.value = 'mac-safari';
    } else{
      screenSize.value = 'mac';
    }
  } else {
    screenSize.value = "extra-large";
  }
  setTranslationLimit();
};

const handleGetStarted = () => {
  router.push("/signup");
};

const content = ref([]);

const initializePageContent = () => {
  content.value = [
    {
      image: require("@/assets/HomePageImages/carousel-image-1.png"),
      title: "Transform Patient Care",
      description:
        "Craft precise reports and prescriptions, improving the patient experience with clarity and detail.",
    },
    {
      image: require("@/assets/HomePageImages/carousel-image-2.png"),
      title: "Streamlined Consultations",
      description:
        "Accelerate your daily tasks with an intuitive interface, helping you focus more on patient care.",
    },
    {
      image: require("@/assets/HomePageImages/carousel-image-3.png"),
      title: "Advanced Session Management",
      description:
        "Easily control when to start, stop, or resume transcriptions based on your consultation needs.",
    },
    {
      image: require("@/assets/HomePageImages/carousel-image-4.png"),
      title: "Seamless Integration",
      description: `${store.MediAssistConfigManager.DomainName} AI seamlessly integrates with your existing systems, ensuring smooth data flow and minimal disruption.`,
    },
  ];

  tabs.value = [
    // { title: `Is ${store.MediAssistConfigManager.DomainName} HIPAA-compliant?`, content: `Yes, ${store.MediAssistConfigManager.DomainName} follows strict HIPAA guidelines to ensure the security and confidentiality of patient data`, value: '0' },
    {
      title: `What is medical scribe software and how does it help doctors?`,
      content: `Medical scribe software like ${store.MediAssistConfigManager.DomainName} automatically transcribes doctor-patient conversations into structured notes, helping physicians save time on documentation and focus more on patient care.`,
      value: "0",
    },
    {
      title: `How is ${store.MediAssistConfigManager.DomainName} different from traditional medical transcription software?`,
      content: `Unlike traditional medical transcription software, ${store.MediAssistConfigManager.DomainName} offers real-time, AI-powered transcription that generates editable SOAP notes instantly-eliminating delays and manual note-taking.`,
      value: "1",
    },
    {
      title: `What are the key benefits of using medical scribe software in clinics and hospitals?`,
      content: `Medical transcription software improves workflow efficiency, reduces physician burnout, increases accuracy in clinical documentation, and accelerates EHR data entry-ultimately enhancing patient care.`,
      value: "2",
    },
    {
      title: `Is medical scribe software secure and compliant with patient privacy standards?`,
      content:
        "Doctors, clinics, hospitals, and healthcare professionals across specialties benefit from medical scribe software by saving time, reducing administrative overhead, and improving note quality.",
      value: "3",
    },
    {
      title: `Does ${store.MediAssistConfigManager.DomainName} integrate with EHR systems?`,
      content: `${store.MediAssistConfigManager.DomainName} ensures data privacy by processing and discarding audio after transcription. While not HIPAA-certified, it does not store any patient data, offering a secure transcription experience.`,
      value: "4",
    },
  ];
};
// const highlightsData = ref([
//   { icon: require('@/assets/icons/transcription-icon.svg'), title: 'Real-Time Transcription', description: 'Transform speech into precise, structured text instantly during consultations.', image: require('@/assets/HomePageImages/speciality-card-1.png') },
//   { icon: require('@/assets/icons/prescription-icon.svg'), title: 'Prescription Generator', description: 'Streamline prescription creation with a smart automated tool.', image: require('@/assets/HomePageImages/speciality-card-2.png') },
//   { icon: require('@/assets/icons/report-icon.svg'), title: 'Report Generation', description: 'Create detailed medical reports effortlessly with one-click functionality.', image: require('@/assets/HomePageImages/speciality-card-3.png') },
// ])

const initialHeroImagePosition = () => {
  if (heroImage.value && hero.value) {
    const parentHeight = hero.value.offsetHeight;
    const heroImageHeight = heroImage.value.offsetHeight;
    if (screenSize.value === 'small') {
      heroImage.value.style.top = `${parentHeight - heroImageHeight * 0.95}px`;
      isFrozen = true;
      heroImage.value.style.transform = `translateY(0px)`;
      return;
    }
    if (screenSize.value === 'mac' || screenSize.value === 'mac-safari') {
      heroImage.value.style.top = `${parentHeight - heroImageHeight * 0.50}px`;
      if(screenSize.value === 'mac-safari'){
        const heroInfo = document.querySelector('.hero-info');
        heroInfo?.style.setProperty("margin-top", '2rem');
      }
      isFrozen = false;
      return;
    }
    heroImage.value.style.top = `${parentHeight - heroImageHeight * 0.35}px`;
    isFrozen = false;
  }
}
const handleScroll = () => {
  isScrolled.value = window.scrollY > 10;

  const scrollY = window.scrollY;
  const maxScroll = window.innerHeight;
  const scrollFraction = Math.min(scrollY / maxScroll, 1);

  if (heroImage.value && heroInfo.value && heroVideo.value) {
    heroVideo.value.style.position = "fixed";
    /*const newTranslateY = -Math.min(scrollFraction * maxScroll, translateLimit);

    if (!isFrozen && Math.abs(newTranslateY) >= translateLimit) {
      isFrozen = true;
      lastScrollPosition = scrollY;
    }

    if (!isFrozen) {
      heroImage.value.style.transform = `translateY(${newTranslateY}px)`;
    }*/

    if (!isFrozen) {
      const newTranslateY = -Math.min(
        scrollFraction * maxScroll,
        translateLimit
      );
      if (Math.abs(newTranslateY) >= translateLimit) {
        isFrozen = true;
        lastScrollPosition = scrollY;
      }
      heroImage.value.style.transform = `translateY(${newTranslateY}px)`;
    }

    heroInfo.value.style.position = "fixed";
    heroInfo.value.style.transform = `scale(${1 - scrollFraction * 0.5})`;
    heroInfo.value.style.opacity = `${1 - scrollFraction * 3}`;
  }
  checkHeroImagePosition(scrollY);
};

const checkHeroImagePosition = (scrollY) => {
  if (heroImage.value) {
    if (screenSize.value === "small" && isFrozen) {
      return;
    }

    if (
      isFrozen &&
      (scrollY < lastScrollPosition || scrollY > lastScrollPosition)
    ) {
      isFrozen = false;
    }

    if (!isFrozen) {
      const maxScroll = window.innerHeight;
      const scrollFraction = Math.min(scrollY / maxScroll, 1);
      const newTranslateY = -Math.min(
        scrollFraction * maxScroll,
        translateLimit
      );
      heroImage.value.style.transform = `translateY(${newTranslateY}px)`;
    }
  }
};

const setTranslationLimit = () => {
  if (heroImage.value) {
    const heroImageHeight = heroImage.value.offsetHeight;
    translateLimit = heroImageHeight * 0.75 - heroImageHeight * 0.35;
  }
};

watch(
  () => store.MediAssistConfigManager.DomainName,
  (newDomainName) => {
    if (newDomainName) {
      initializePageContent();
    }
  }
);

const createResizeObserver = () => {
  resizeObserver = new ResizeObserver(() => {
    console.log("resize observer instance created.");
    initialHeroImagePosition();
  });

  if (hero.value) {
    resizeObserver.observe(hero.value);
  }
};

const handleRequestDemo = (modalType) => {
  localStorage.setItem("modalType", modalType);
  router.push("/request-demo");
};

onMounted(() => {
  createResizeObserver();
  updateScreenSize();
  initializePageContent();
  initialHeroImagePosition();
  window.addEventListener("scroll", handleScroll);
  window.addEventListener("resize", updateScreenSize);
});

onUnmounted(() => {
  if (resizeObserver && hero.value) {
    resizeObserver.unobserve(hero.value);
  }
  window.removeEventListener("scroll", handleScroll);
});
</script>
<template>
  <section id="hero" class="hero" ref="hero" v-scroll-reveal>
    <div class="bg-layer">
      <video autoplay muted loop class="hero-video" ref="heroVideo">
        <source src="@/assets/background-video.mp4" type="video/mp4" />
      </video>
      <div class="overlay-layer">
        <img
          v-if="screenSize === 'small'"
          src="@/assets/HomePageImages/overlay-shape-1-sm.svg"
          alt="shapes"
        />
        <img
          v-else
          src="@/assets/HomePageImages/overlay-shape-1.svg"
          alt="shapes"
        />
      </div>
    </div>
    <div class="hero-info" ref="heroInfo">
      <div class="content-wrapper" ref="contentWrapper">
        <h1 class="h1-semibold" v-scroll-reveal>
          <!-- {{store.MediAssistConfigManager.DomainName}} AI <br><span>Redefining Medical Consultations</span> -->
          AI-Powered Medical Scribe Software for Physicians & Clinicians
        </h1>
        <p class="lg-regular" v-scroll-reveal>
          Perfect real-time voice-to-text transcription for healthcare
          professionals, including doctors, clinics, and hospitals.
        </p>
        <button
          @click="handleRequestDemo('request-demo')"
          class="btn btn-primary"
        >
          <div class="btn-background">
            <div class="inside-text">Book a Demo</div>
          </div>
          <div class="border-gradient-fn" />
        </button>
      </div>
    </div>
    <div class="hero-image" id="heroImage" ref="heroImage">
      <!--      <picture>
        <img class="img-sm" src="@/assets/HomePageImages/hero-image-sm.svg" alt="hero-image">
      </picture>-->
      <img
        class="img-sm"
        src="@/assets/HomePageImages/hero-image-lg.jpg"
        alt="hero-image"
      />
    </div>
    <div class="clip-layer">
      <img
        v-if="screenSize === 'small'"
        src="@/assets/HomePageImages/clip-shape-sm.svg"
        alt="clip shape"
      />
      <img
        v-else
        src="@/assets/HomePageImages/clip-shape.svg"
        alt="clip shape"
      />
    </div>
  </section>
  <section id="speciality" class="speciality" ref="speciality">
    <div class="content">
      <div class="container-top">
        <img
          src="@/assets/HomePageImages/medi-notex-ai.png"
          alt="medinotex-icon-image"
        />
        <div class="frame">
          <div class="div">
            <div class="div-wrapper" v-scroll-reveal>
              <h2 class="text-wrapper h2-semibold">
                Revolutionizing Medical Documentation with AI
              </h2>
            </div>

            <p class="are-you-a-healthcare">
              <h7 class="span h7-regular" v-scroll-reveal>
                Are you a healthcare provider burdened by excessive
                documentation?
                <br />
              </h7>

              <h7
                class="text-wrapper-2 h7-regular"
                v-scroll-reveal="{ distance: '50px', delay: 700 }"
              >
                <br />
                {{ store.MediAssistConfigManager.DomainName }} is an AI-powered
                medical scribe software solution designed to automate SOAP note
                generation, allowing you to focus more on patient care. Our
                advanced real-time voice-to-text technology accurately
                transcribes consultations, producing precise and timely medical
                notes. Eliminate manual paperwork and enhance efficiency with
                our intelligent documentation system. Request a demo today!
              </h7>
            </p>
          </div>
        </div>
      </div>
    </div>
  </section>
  <section v-scroll-reveal id="highlights" class="highlights" ref="highlights">
    <div class="container">
      <div class="rectangle">
        <video autoplay muted controls loop class="hero-video">
          <source src="@/assets/highlights.mp4" type="video/mp4" />
        </video>
      </div>
    </div>
  </section>
  <section id="howItWorks" ref="howItWorks">
    <div class="frame">
      <div class="div">
        <div class="frame-wrapper">
          <div class="div-2">
            <div class="div-wrapper">
              <div class="frame-wrapper-2">
                <div class="div-wrapper-2">
                  <h2 class="text-wrapper h2-semibold" v-scroll-reveal>
                    How {{ store.MediAssistConfigManager.DomainName }} Works?
                  </h2>
                </div>
              </div>
            </div>

            <div class="div-3">
              <p class="p lg-regular" v-scroll-reveal>
                Are you a healthcare provider burdened by excessive
                documentation? {{ store.MediAssistConfigManager.DomainName }} is
                an AI-powered medical scribe software solution designed to
                automate SOAP.
              </p>

              <button
                class="btn btn-primary"
                style="width: 150px; cursor: pointer"
                @click="handleGetStarted"
                v-scroll-reveal
              >
                <div class="btn-background">
                  <div class="inside-text">
                    <span>Get Started</span>
                  </div>
                </div>
                <div class="border-gradient-fn"></div>
              </button>
            </div>
          </div>
        </div>

        <div class="div-4">
          <div class="div-5">
            <div class="div-6">
              <div class="div-7">
                <div class="div-wrapper-4">
                  <div class="text-wrapper-3">1</div>
                </div>

                <img
                  class="line"
                  alt="Line"
                  src="@/assets/HomePageImages/line-2.png"
                />

                <img
                  class="img"
                  alt="Line"
                  src="@/assets/HomePageImages/line-3.png"
                />
              </div>

              <div
                class="div-8"
                v-scroll-reveal="{
                  origin: 'bottom',
                  distance: '50px',
                  delay: 500,
                }"
              >
                <div class="div-wrapper-5">
                  <h6 class="text-wrapper-3 h6-medium">
                    Real-Time Transcription
                  </h6>
                </div>

                <p class="text-wrapper-4 lg-regular">
                  {{ store.MediAssistConfigManager.DomainName }} uses
                  cutting-edge AI to instantly convert real-time spoken words
                  into accurate, comprehensive medical notes.
                </p>
              </div>
            </div>

            <div
              class="div-6"
              v-scroll-reveal="{
                origin: 'bottom',
                distance: '50px',
                delay: 600,
              }"
            >
              <div class="div-7">
                <div class="div-wrapper-4">
                  <div class="text-wrapper-3">2</div>
                </div>
              </div>

              <div
                class="div-8"
                v-scroll-reveal="{
                  origin: 'bottom',
                  distance: '50px',
                  delay: 700,
                }"
              >
                <div class="div-wrapper-5">
                  <h6 class="text-wrapper-3 h6-medium">
                    Potential Diagnosis Feature Control
                  </h6>
                </div>

                <p class="text-wrapper-4 lg-regular">
                  Doctors have the flexibility to enable or disable the
                  Potential Diagnosis feature based on their individual
                  preferences and clinical requirements
                </p>
              </div>
            </div>

            <div
              class="div-6"
              v-scroll-reveal="{
                origin: 'bottom',
                distance: '50px',
                delay: 800,
              }"
            >
              <div class="div-7">
                <div class="div-wrapper-4">
                  <div class="text-wrapper-3">3</div>
                </div>
              </div>

              <div class="div-8">
                <div class="div-9">
                  <div class="div-wrapper-5">
                    <h6 class="text-wrapper-3 h6-medium">
                      Automated Prescription Generation
                    </h6>
                  </div>

                  <p class="text-wrapper-4 lg-regular">
                    Our intelligent system analyses recorded conversations,
                    extracting essential information to generate precise medical
                    prescriptions.
                  </p>
                </div>
              </div>

              <img
                class="line-2"
                alt="Line"
                src="@/assets/HomePageImages/line-3-2.png"
              />
            </div>

            <div
              class="div-6"
              v-scroll-reveal="{
                origin: 'bottom',
                distance: '50px',
                delay: 900,
              }"
            >
              <div class="div-7">
                <div class="div-wrapper-4">
                  <div class="text-wrapper-3">4</div>
                </div>
              </div>

              <div class="div-8">
                <div class="div-9">
                  <div class="div-wrapper-5">
                    <h6 class="text-wrapper-3 h6-medium">
                      Comprehensive Report Creation
                    </h6>
                  </div>

                  <p class="text-wrapper-4 lg-regular">
                    {{ store.MediAssistConfigManager.DomainName }} generates
                    detailed medical reports with hospital details, doctor
                    signatures, SOAP notes, and progress summaries for seamless
                    documentation.
                  </p>
                </div>
              </div>
            </div>
          </div>

          <div class="group">
            <SlidingFeatureCard></SlidingFeatureCard>
          </div>
        </div>
      </div>
    </div>
  </section>

  <section id="featureCards" class="benefits-section" ref="featureCards">
    <BenifitsCard></BenifitsCard>
  </section>

  <section class="accordian-secion" id="accordian" ref="accordian">
    <AccordianComponent :tabs="tabs" />
  </section>

  <section
    id="bookDemo"
    class="book-demo-section"
    ref="bookDemo"
    v-scroll-reveal="{ distance: '50px', delay: 400 }"
  >
    <BookDemoComponent></BookDemoComponent>
  </section>
  <ScheduleDemoPopUp
    :is-visible="showScheduleDemoPopUp"
    modal-type="request-demo"
    title="Request your Demo"
    description="Fill out your details to schedule a personalized demo"
    submitButtonText="Request your Demo"
    @close="showScheduleDemoPopUp = false"
  />
</template>

<style scoped lang="css">
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}
.hero {
  width: 100%;
  height: 100vh;
  color: white;
  position: relative;
  overflow: hidden;
  display: flex;
  justify-content: center;

  /* @media (max-width: 744px) {
    height: 655px;
  } */
  /* @media (min-height: 840px) and (max-width: 744px) {
  height: 588px;
  }

  @media (min-width: 745px) and (min-height: 558px) {
  height: 655px;
  } */

  @media (min-height: 655px) and (max-width: 744px) {
    height: 40.9375rem;
  }

  & .bg-layer {
    position: absolute;
    z-index: -10;
    width: 100%;
    height: inherit;
    background: #050613;

    &::before {
      content: "";
      position: absolute;
      z-index: 1;
      width: 100%;
      height: inherit;
      background: linear-gradient(
        93deg,
        #060815 2.2%,
        rgba(6, 8, 21, 0.5) 97.48%
      );
    }

    & .overlay-layer {
      position: absolute;
      z-index: 2;
      width: 100vw;
      height: inherit;

      & img {
        position: absolute;
        bottom: -4.78125rem;
        width: inherit;
        height: auto;
        z-index: 3;

        @media (max-width: 744px) {
          bottom: -7.78125rem;
        }
      }
    }

    & .hero-video {
      position: absolute;
      z-index: -2;
      width: 100%;
      height: 100vh;
      top: 0;
      right: 0;
      object-fit: fill;
    }
  }

  & .clip-layer {
    position: absolute;
    width: calc(100%);
    height: auto;
    z-index: 10;
    bottom: -3.125rem;
    transform: scaleX(1.01);
    background: transparent;

    @media (max-width: 744px) {
      bottom: -7.125rem;
    }

    & img {
      width: 100%;
      height: 100%;
    }
  }

  .hero-info {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100%;
    gap: 4.8125rem;
    padding-top: 9.375rem;
    transition: transfrom 0.3s ease, opacity 0.3s ease;
    position: relative;

    & .content-wrapper {
      width: 896px;
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 1.5rem;
      min-width: 21.3125rem;

      @media (min-height: 840px) {
        gap: 2.5rem;
      }

      @media (min-width: 426px) and (max-width: 744px) {
        max-width: 30rem;
      }

      @media (min-width: 745px) {
        max-width: none;
      }

      h1 {
        text-align: center;
        width: 100%;
        padding-inline: 1rem;

        @media (max-width: 480px) {
          font-size: 1.5rem;
          line-height: 2.25rem;
          color: #fff;
          text-align: center;
          font-style: normal;
          width: 18ch;
        }
      }

      div.description {
        font-size: 16px;
        font-weight: 400;
        font-style: normal;
        line-height: 150%;
        text-align: center;

        @media (max-width: 480px) {
          font-size: 1.5rem;
          line-height: 2.25rem;
          color: #fff;
          text-align: center;
          font-style: normal;
        }

        @media (max-width: 744px) {
          font-size: 1.5rem;
          font-weight: 400;
          line-height: 2.25rem;
          text-align: center;
        }
      }

      p {
        text-align: center;
        padding-inline: 1rem;

        @media (max-width: 480px) {
          width: 30ch;
        }
      }

      button {
        width: 165px;
        height: 40px;
        cursor: pointer;
      }
    }
  }
  .hero-image {
    width: fit-content;
    height: fit-content;
    object-fit: fill;
    margin-inline: auto;
    transition: transfrom 0.3s ease, box-shadow 0.3s ease;
    position: absolute;
    top: 100%;
    display: flex;
    justify-content: center;
    border-radius: 2.25rem;

    &::before {
      position: absolute;
      top: 0;
      left: 0;
      z-index: -2;
      width: 100%;
      height: 100%;
      pointer-events: none;
      content: "";
      /* background: #1949BF;
      filter: blur(11.0625rem);*/
      border-radius: inherit;
      opacity: 0.85;
      transform: scale(1.05);
    }

    & img {
      width: 1046px;
      height: auto;
      aspect-ratio: 1.4 /1;
      border-radius: 16px;

      @media (max-width: 480px) {
        width: 287px;
      }

      @media (min-width: 481px) and (max-width: 768px) {
        width: 425px;
      }

      @media (min-width: 769px) and (max-width: 1200px) {
        width: 725px;
      }
    }
  }
}

.medinotex-icon-image {
  width: 86.54609680175781px;
  height: 92.5091781616211px;
}

.speciality {
  width: 100%;
  height: fit-content;
  padding-block: 5rem 2rem;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 3.5625rem;
  position: relative;
  background: white;
  padding-inline: 1rem;

  @media (min-width: 481px) and (max-width: 1440px) {
    padding-block: 5rem 3rem;
    padding-inline: 4rem;
  }

  @media (max-width: 744px) {
    padding-inline: 1rem;
  }

  & .content {
    width: 100%;
    height: inherit;
    display: flex;
    flex-direction: column;
    align-items: center;

    @media (min-width: 481px) {
      width: 100%;
      height: inherit;
      display: flex;
      flex-direction: column;
      align-items: center;
    }

    @media (min-width: 1440px) {
      max-width: 1280px;
      margin-inline: auto;
    }

    & .container-top {
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: center;
      gap: 0.875rem;
      max-width: 58.375rem;

      @media (min-width: 481px) {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        gap: 0.875rem;
        max-width: 58.375rem;
      }

      & img {
        width: 86.54609680175781px;
        height: 92.5091781616211px;
      }

      .frame {
        align-items: center;
        display: flex;
        flex-direction: column;
        gap: 32px;
        position: relative;
      }

      .frame .div {
        align-items: center;
        align-self: stretch;
        display: flex;
        flex: 0 0 auto;
        flex-direction: column;
        gap: 16px;
        position: relative;
        width: 100%;
      }

      .frame .div-wrapper {
        align-items: center;
        align-self: stretch;
        display: flex;
        flex: 0 0 auto;
        flex-direction: column;
        gap: 14px;
        position: relative;
        width: 100%;
      }

      .frame .text-wrapper {
        color: #1c1c1c;
        font-size: 38px;
        font-weight: 600;
        letter-spacing: 0;
        line-height: 57px;
        margin-top: -1px;
        position: relative;
        text-align: center;
        white-space: nowrap;
        width: fit-content;
      }

      .frame .are-you-a-healthcare {
        color: transparent;
        position: relative;
        text-align: center;
        width: 917.29px;
      }

      .frame .span {
        color: #1c1c1c;
      }

      .frame .text-wrapper-2 {
        color: #5f5f5f;
        font-size: 16px;
        line-height: 24px;
      }

      .frame .rectangle {
        background: linear-gradient(
          180deg,
          rgba(0, 0, 0, 0) 0%,
          rgba(0, 0, 0, 0.8) 100%
        );
        border-radius: 24px;
        height: 587.32px;
        position: relative;
        width: 1029.68px;
      }
    }

    & .container-bottom {
      display: flex;
      flex-direction: column;
      height: auto;
      width: 100%;
      gap: 0.75rem;
      margin-top: 3.75rem;

      @media (min-width: 481px) {
        flex-direction: row;
        height: auto;
        width: 100%;
        gap: 0.75rem;
        margin-top: 3.75rem;
      }

      & .card {
        display: flex;
        flex-direction: column;
        align-items: start;
        border-radius: 0.8125rem;
        background-color: #fafafa;
        padding: 1.5rem;
        width: 100%;
        height: auto;
        position: relative;

        @media (min-width: 481px) {
          display: flex;
          flex-direction: column;
          align-items: start;
          border-radius: 0.8125rem;
          background-color: #fafafa;
          padding: 1.5rem;
          width: 33%;
          height: auto;
          position: relative;
        }

        & .card-content {
          margin-top: 1rem;
          flex-grow: 1;

          @media (min-width: 481px) {
            margin-top: 1rem;
            flex-grow: 1;
          }
        }

        & .card-footer {
          width: 100%;
          height: auto;
          margin-top: 2.0625rem;

          @media (min-width: 481px) {
            width: 100%;
            height: auto;
            margin-top: 2.0625rem;
          }

          & .image-wrapper {
            width: inherit;
            height: inherit;
            position: relative;
            overflow: clip;

            @media (min-width: 481px) {
              width: inherit;
              height: 13.75rem;
              position: relative;
              overflow: clip;
            }

            & img {
              width: inherit;
              border-radius: 0.5rem;
              background: #fff;
              box-shadow: 0 0.164875rem 0.5770625rem -0.04125rem #e5f1ff;

              @media (min-width: 481px) {
                width: inherit;
                border-radius: 0.5rem;
                background: #fff;
                box-shadow: 0 0.164875rem 0.5770625rem -0.04125rem #e5f1ff;
              }
            }

            & .shadow {
              position: absolute;
              width: 100%;
              height: 5.125rem;
              bottom: 0;
              left: 0;
              background: linear-gradient(
                180deg,
                rgba(250, 250, 250, 0) 0%,
                #fafafa 75.44%,
                #fafafa 107.78%
              );
              z-index: 100;

              @media (min-width: 481px) {
                position: absolute;
                width: 100%;
                height: 5.125rem;
                bottom: 0;
                left: 0;
                background: linear-gradient(
                  180deg,
                  rgba(250, 250, 250, 0) 0%,
                  #fafafa 75.44%,
                  #fafafa 107.78%
                );
                z-index: 100;
              }
            }
          }
        }

        & .card-icon {
          @media (min-width: 481px) {
            width: 100%;
            height: 3.75rem;
          }
        }

        & .card-title {
          color: #000;
          font-size: 1.375rem;
          font-style: normal;
          font-weight: 400;
          line-height: 150%;
        }

        & .card-description {
          margin-top: 0.3125rem;
          color: #4f4f4f;
          font-size: 0.875rem;
          font-style: normal;
          font-weight: 400;
          line-height: 150%;
        }

        & > img {
          margin-top: 1.625rem;
          width: 100%;
          height: 100%;
          object-fit: cover;
        }
      }
    }
  }
}

.highlights {
  width: 100%;
  height: fit-content;
  background: white;
  position: relative;

  & .container {
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;

    @media (min-width: 769px) {
      flex-direction: row;
    }

    @media (min-width: 1440px) {
      max-width: 1280px;
      margin-inline: auto;
    }

    & .content {
      width: 100%;
      height: 100%;
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: flex-start;
      position: relative;
      padding: 2.25rem 1rem;

      @media (min-width: 769px) {
        width: 50%;
        padding: 0 0;
      }

      &::before {
        content: "";
        position: absolute;
        top: 4.433rem;
        right: 2.552rem;
        width: 16.4375rem;
        height: 12rem;
        background: url("@/assets/HomePageImages/overlay-shape-2.svg") no-repeat;
        background-size: cover;
      }

      & .inner-container {
        height: fit-content;
        width: 100%;
        display: flex;
        flex-direction: column;
        gap: 1.25rem;

        @media (min-width: 481px) {
          gap: 1rem;
          padding-left: 4rem;
        }

        @media (min-width: 1440px) {
          padding-left: 0;
        }

        & h3,
        & p {
          max-width: 100%;

          @media (min-width: 769px) {
            max-width: 27.3125rem;
          }
        }

        & h3 {
          color: #fff;
          font-size: 0.875rem;
          font-style: normal;
          font-weight: 600;
          line-height: 1.3125rem;
          letter-spacing: 0.175rem;
          width: 638px;

          @media (min-width: 769px) {
            font-size: 1.125rem;
            line-height: 1.6875rem;
            letter-spacing: 0.225rem;
          }
        }

        & p {
          color: #fff;
          font-size: 1.5rem;
          font-style: normal;
          font-weight: 400;
          line-height: 2.25rem;

          @media (min-width: 769px) {
            font-size: 2.375rem;
            line-height: 3.5625rem;
          }
        }
      }
    }

    .hero-video {
      position: absolute;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      object-fit: cover;
      border-radius: 24px;
    }

    .rectangle {
      background: linear-gradient(
        180deg,
        rgba(0, 0, 0, 0) 0%,
        rgba(0, 0, 0, 0.8) 100%
      );
      border-radius: 24px;
      height: 587.32px;
      position: relative;
      width: 1029.68px;
      overflow: hidden;
    }

    & .video {
      display: flex;
      flex-direction: column;
      width: 100%;
      height: 100%;
      justify-content: center;
      /*@media (min-width: 769px) {
        width: 100%;
      }*/

      @media (min-width: 1281px) {
        border-radius: 0.8125rem;
      }

      & video {
        width: fit-content;
        height: inherit;
        margin-inline: auto;

        @media (max-width: 1280px) {
          width: inherit;
          height: inherit;
        }

        @media (min-width: 1281px) {
          width: 1280px;
          height: auto;
          aspect-ratio: auto;
          border-radius: 0.8125rem;
        }
      }
    }
  }
}

.benefits-section {
  height: 691px;
  background: white url("@/assets/HomePageImages/Benefits/mask-group.jpg") center no-repeat;
  background-size: cover;
}

.accordian-secion {
  height: 755px;
  margin: 0;
  background-color: #f7fbff;
}

#howItWorks {
  background: white;
  padding-block: 4rem 0;

  .frame {
    align-items: center;
    background-color: #f7fbff;
    display: flex;
    flex-direction: column;
    gap: 10px;
    padding: 60px 100px;
    position: relative;
    width: 100%;
  }

  .frame .div {
    align-items: flex-start;
    display: flex;
    flex: 0 0 auto;
    flex-direction: column;
    gap: 59.81px;
    margin-right: -1px;
    position: relative;
    width: 1280px;
  }

  .frame .frame-wrapper {
    align-items: flex-end;
    align-self: stretch;
    display: flex;
    flex: 0 0 auto;
    flex-direction: column;
    gap: 24px;
    justify-content: center;
    position: relative;
    width: 100%;
  }

  .frame .div-2 {
    align-items: flex-start;
    align-self: stretch;
    display: flex;
    flex: 0 0 auto;
    flex-direction: column;
    gap: 24px;
    position: relative;
    width: 100%;
  }

  .frame .div-wrapper {
    align-items: flex-start;
    align-self: stretch;
    display: flex;
    flex: 0 0 auto;
    flex-direction: column;
    gap: 15.95px;
    justify-content: center;
    position: relative;
    width: 100%;
  }

  .frame .frame-wrapper-2 {
    align-items: flex-start;
    align-self: stretch;
    display: flex;
    flex: 0 0 auto;
    gap: 15.95px;
    position: relative;
    width: 100%;
  }

  .frame .div-wrapper-2 {
    align-items: center;
    display: flex;
    flex: 1;
    flex-direction: column;
    flex-grow: 1;
    gap: 31.9px;
    justify-content: center;
    position: relative;
  }

  .frame .text-wrapper {
    align-self: stretch;
    color: #1c1c1c;
    margin-top: -1px;
    position: relative;
  }

  .frame .div-3 {
    align-items: center;
    align-self: stretch;
    display: flex;
    flex: 0 0 auto;
    justify-content: space-between;
    position: relative;
    width: 100%;
  }

  .frame .p {
    color: #5f5f5f;
    margin-top: -1px;
    position: relative;
    width: 735.48px;
  }

  .frame .frame-wrapper-3 {
    align-items: center;
    background-color: #020617;
    border: none;
    border-radius: 7.82px;
    display: inline-flex;
    flex: 0 0 auto;
    flex-direction: column;
    height: 40px;
    justify-content: center;
    padding: 16px;
    position: relative;
  }

  .frame .frame-wrapper-3::before {
    -webkit-mask: linear-gradient(#fff 0 0) content-box,
      linear-gradient(#fff 0 0);
    background: linear-gradient(
      to bottom,
      rgb(9.24, 59.85, 118.76),
      rgb(96.03, 165.3, 242.27) 25%,
      rgb(64.11, 124.51, 192.97) 69%,
      rgb(96.03, 165.3, 242.27) 100%
    );
    border-radius: 7.82px;
    content: "";
    inset: 0;
    mask-composite: exclude;
    padding: 0.98px;
    position: absolute;
    z-index: 1;
  }

  .frame .div-wrapper-3 {
    align-items: center;
    display: inline-flex;
    gap: 5px;
    height: 23.14px;
    justify-content: center;
    margin-bottom: -7.57px;
    margin-top: -7.57px;
    position: relative;
  }

  .frame .text-wrapper-2 {
    color: #606060;
    margin-top: -1.68px;
    position: relative;
    white-space: nowrap;
    width: fit-content;
  }

  .frame .div-4 {
    align-items: flex-start;
    align-self: stretch;
    display: flex;
    flex: 0 0 auto;
    justify-content: space-between;
    position: relative;
    width: 100%;
  }

  .frame .div-5 {
    align-items: flex-start;
    display: flex;
    flex-direction: column;
    gap: 40px;
    position: relative;
    width: 562.19px;
  }

  .frame .div-6 {
    align-items: flex-start;
    align-self: stretch;
    display: flex;
    flex: 0 0 auto;
    gap: 31.9px;
    position: relative;
    width: 100%;
  }

  .frame .div-7 {
    background: #ffffff;
    align-items: center;
    border: 1px solid;
    border-color: #000000;
    border-radius: 23.92px;
    display: flex;
    flex-direction: column;
    gap: 23.92px;
    height: 47.85px;
    justify-content: center;
    padding: 15.95px;
    position: relative;
    width: 47.85px;
  }

  .frame .div-wrapper-4 {
    align-items: flex-start;
    display: inline-flex;
    flex: 0 0 auto;
    flex-direction: column;
    gap: 9.74px;
    margin-bottom: -8.03px;
    margin-top: -8.03px;
    position: relative;
  }

  .frame .text-wrapper-3 {
    align-self: stretch;
    color: #1c1c1c;
    margin-top: -0.65px;
    position: relative;
  }

  .frame .line {
    height: 100px;
    left: 24px;
    object-fit: cover;
    position: absolute;
    top: 48px;
    width: 1px;
  }

  .frame .img {
    height: 120px;
    left: 24px;
    object-fit: cover;
    position: absolute;
    top: 186px;
    width: 1px;
  }

  .frame .div-8 {
    align-items: flex-start;
    display: flex;
    flex-direction: column;
    gap: 10px;
    position: relative;
    width: 482.44px;
  }

  .frame .div-wrapper-5 {
    align-items: flex-start;
    display: inline-flex;
    flex: 0 0 auto;
    flex-direction: column;
    gap: 9.74px;
    position: relative;
  }

  .frame .text-wrapper-4 {
    align-self: stretch;
    color: #5f5f5f;
    position: relative;
  }

  .frame .div-9 {
    align-items: flex-start;
    align-self: stretch;
    display: flex;
    flex: 0 0 auto;
    flex-direction: column;
    gap: 16px;
    position: relative;
    width: 100%;
  }

  .frame .line-2 {
    height: 118px;
    left: 24px;
    object-fit: cover;
    position: absolute;
    top: 48px;
    width: 1px;
  }

  @media (max-width: 468px) {
    padding-block-end: 1rem;
  }
}

.book-demo-section {
  height: 404px;
  background: white;
  background-size: cover;
  position: relative;
  & .center-image {
    padding: 60px;
    display: flex;
    justify-content: center;
  }
  & .container {
    width: 1241px;
    height: 310px;
    background: white
      url("@/assets/HomePageImages/book-demo-background-image.jpg") center
      no-repeat;
    background-size: cover;
    border-radius: 30px;

    & .gradient-layer {
      height: inherit;
      background: linear-gradient(
        266.01deg,
        rgba(52, 115, 209, 0) 0.98%,
        #0e2766 98.85%
      );
      border-radius: 30px;

      & .content {
        color: #fff;
        width: 100%;
        height: inherit;
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        max-width: 1280px;
        padding-inline: 5rem;
        padding-block: 3rem;

        @media (min-width: 481px) and (max-width: 1280px) {
          padding-inline: 3.125rem;
          padding-block: 0;
          justify-content: center;
          margin-inline: auto;
        }

        @media (max-width: 744px) {
          padding-inline: 1rem;
        }

        @media (min-width: 1281px) {
          margin-inline: auto;
        }

        & h3 {
          color: inherit;
          font-size: 1.5rem;
          font-style: normal;
          font-weight: 400;
          line-height: 150%;
          width: 14ch;

          @media (min-width: 481px) {
            width: fit-content;
            color: inherit;
            font-size: 2.625rem;
            font-style: normal;
            font-weight: 400;
            line-height: 4.463rem;
          }
        }

        & p {
          color: inherit;
          font-size: 0.875rem;
          font-style: normal;
          font-weight: 400;
          line-height: 150%;

          @media (min-width: 481px) {
            color: inherit;
            font-size: 1.3125rem;
            font-style: normal;
            font-weight: 400;
            line-height: 1.969rem;
          }
        }

        & .btn {
          margin-top: 1.5rem;
          width: 150px;
          cursor: pointer;
        }
      }
    }
  }
}
</style>
