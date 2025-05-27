<script setup>
import CarouselCard from '@/components/carousel/CarouselCard.vue';
import ScheduleDemoPopUp from '@/Pages/modals/GenericModal.vue';
import { onMounted, onUnmounted, ref, computed, watch } from "vue";
import { useRouter, useRoute } from 'vue-router';
import { useMyStore } from '@/store/store.ts';

const CAROUSEL_TYPE = {
  DEFAULT: 1,
  SPECIALITY: 2
}

const router = useRouter();
const route = useRoute();
const store = useMyStore();

const hero = ref(null);
const heroImage = ref(null);
const heroInfo = ref(null);
const contentWrapper = ref(null);
const heroVideo = ref(null);
const isScrolled = ref(false);
const screenSize = ref('large');
const showScheduleDemoPopUp = ref(false);
let translateLimit = 0;
let lastScrollPosition = 0;
let isFrozen = false;
let resizeObserver = null;

const speciality = ref(null);
const highlights = ref(null);
const features = ref(null);
const getStarted = ref(null);

const updateScreenSize = () => {
  if (window.matchMedia("(max-width: 480px)").matches) {
    screenSize.value = 'small';
  } else if(window.matchMedia("(min-width: 481px) and (max-width: 768px)").matches) {
    screenSize.value = 'medium';
  } else if (window.matchMedia("(min-width: 769px) and (max-width: 1440px)").matches) {
    screenSize.value = 'large';
  } else {
    screenSize.value = 'extra-large';
  }
  setTranslationLimit();
};

const indicators = computed(() => screenSize.value === 'small' || screenSize.value === 'medium');

const isHighlightsCarousel = computed(() => {
  return screenSize.value === 'small' || screenSize.value === 'medium';
});

const handleGetStarted = () => {
  router.push('/signup');
};

const slidesPerView = computed(() => {
  if (screenSize.value === 'small' || screenSize.value === 'medium') {
    return 1;
  } else {
    return 2;
  }
});

const content = ref([]);

const initializePageContent = () => {
  content.value = [
    {
      image: require('@/assets/HomePageImages/carousel-image-1.png'),
      title: 'Transform Patient Care',
      description: 'Craft precise reports and prescriptions, improving the patient experience with clarity and detail.'
    },
    {
      image: require('@/assets/HomePageImages/carousel-image-2.png'),
      title: 'Streamlined Consultations',
      description: 'Accelerate your daily tasks with an intuitive interface, helping you focus more on patient care.'
    },
    {
      image: require('@/assets/HomePageImages/carousel-image-3.png'),
      title: 'Advanced Session Management',
      description: 'Easily control when to start, stop, or resume transcriptions based on your consultation needs.'
    },
    {
      image: require('@/assets/HomePageImages/carousel-image-4.png'),
      title: 'Seamless Integration',
      description: `${store.MediAssistConfigManager.DomainName} AI seamlessly integrates with your existing systems, ensuring smooth data flow and minimal disruption.`
    },
  ];
};
const highlightsData = ref([
  { icon: require('@/assets/icons/transcription-icon.svg'), title: 'Real-Time Transcription', description: 'Transform speech into precise, structured text instantly during consultations.', image: require('@/assets/HomePageImages/speciality-card-1.png') },
  { icon: require('@/assets/icons/prescription-icon.svg'), title: 'Prescription Generator', description: 'Streamline prescription creation with a smart automated tool.', image: require('@/assets/HomePageImages/speciality-card-2.png') },
  { icon: require('@/assets/icons/report-icon.svg'), title: 'Report Generation', description: 'Create detailed medical reports effortlessly with one-click functionality.', image: require('@/assets/HomePageImages/speciality-card-3.png') },
])

const initialHeroImagePosition = () => {
  if (heroImage.value && hero.value) {
    const parentHeight = hero.value.offsetHeight;
    const heroImageHeight = heroImage.value.offsetHeight;
    if (screenSize.value === 'small') {
      heroImage.value.style.top = `${parentHeight - heroImageHeight * 0.75}px`;
      isFrozen = true;
      heroImage.value.style.transform = `translateY(0px)`;
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
    heroVideo.value.style.position = 'fixed';

    if (!isFrozen) {
      const newTranslateY = -Math.min(scrollFraction * maxScroll, translateLimit);
      if (Math.abs(newTranslateY) >= translateLimit) {
        isFrozen = true;
        lastScrollPosition = scrollY;
      }
      heroImage.value.style.transform = `translateY(${newTranslateY}px)`;
    }

    heroInfo.value.style.position = 'fixed';
    heroInfo.value.style.transform = `scale(${1 - scrollFraction * 0.5})`;
    heroInfo.value.style.opacity = `${1 - scrollFraction * 3}`;

  }
  checkHeroImagePosition(scrollY);
};

const checkHeroImagePosition = (scrollY) => {
  if (heroImage.value) {
    if (
        screenSize.value === 'small' &&
        isFrozen
    ) {
      return;
    }

    if (
        isFrozen &&
        ((scrollY < lastScrollPosition) ||
            (scrollY > lastScrollPosition))
    ) {
      isFrozen = false;
    }

    if (!isFrozen) {
      const maxScroll = window.innerHeight;
      const scrollFraction = Math.min(scrollY / maxScroll, 1);
      const newTranslateY = -Math.min(scrollFraction * maxScroll, translateLimit);
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

const handleIntersect = (entries) => {
  const headerElement = document.querySelector(".header");
  const navMenu = document.querySelector(".nav-menu");
  const signupButton = document.querySelector(".signup");
  const branding = document.querySelector("#branding");
  const hamburger = document.querySelector("button#hamburger svg");
  const hamburgerNavManu = document.querySelector("#ham-nav-menu");
  const signupBottonNavMenu = document.querySelector("#signup-button");

  const intersectingEntries = entries.filter((entry) => entry.isIntersecting);
  if (intersectingEntries.length > 0) {
    const lastIntersecting = intersectingEntries[intersectingEntries.length - 1];
    if (
        lastIntersecting.target.id === "speciality" ||
        lastIntersecting.target.id === "features"
    ) {
      headerElement.style.backgroundColor = "transparent";
      navMenu.style.color = "#0A2144";
      signupButton.style.color = "#0A2144";
      hamburger.style.fill = "#0A2144";
      if(hamburgerNavManu){
        hamburgerNavManu.style.color = "#0A2144";
      }
      if(signupBottonNavMenu){
        signupBottonNavMenu.style.color = "#0A2144";
      }
      branding.style.color = '';
    } else if (lastIntersecting.target.id === "highlights" || lastIntersecting.target.id === "get-started") {
      headerElement.style.backgroundColor = "transparent";
      navMenu.style.color = "white";
      signupButton.style.color = "white";
      if(hamburgerNavManu){
        hamburgerNavManu.style.color = "white";
      }
      if(signupBottonNavMenu){
        signupBottonNavMenu.style.color = "white";
      }
      branding.style.color = "white";
      hamburger.style.fill = "white";
    }
  } else {
    if (route.path === '/chooseyourplan' || route.path === '/terms-and-conditions' || route.path === '/privacy-policy' || route.path === '/license-agreement') {
      headerElement.style.backgroundColor = "transparent";
      navMenu.style.color = "#0A2144";
      signupButton.style.color = "#0A2144";
      branding.style.color = '#163666'
      hamburger.style.fill = "#0A2144";
      return;
    }
    if (headerElement && navMenu && signupButton) {
      headerElement.style.backgroundColor = "rgba(8, 19, 50, 0.4)";
      navMenu.style.color = "white";
      signupButton.style.color = "white";
      branding.style.color = 'white';
      hamburger.style.fill = 'white';
      if(hamburgerNavManu){
        hamburgerNavManu.style.color = "white";
      }
      if(signupBottonNavMenu){
        signupBottonNavMenu.style.color = "white";
      }
    }
  }
};

const createObserver = () => {
  let observer;
  let options = {
    root: null,
    rootMargin: `-80px 0px -${window.innerHeight}px 0px`,
    threshold: 0,
  };
  observer = new IntersectionObserver(handleIntersect, options);
  console.log(observer);
  observer.observe(speciality.value);
  observer.observe(highlights.value);
  observer.observe(features.value);
  observer.observe(getStarted.value);
};

const createResizeObserver = () => {
  resizeObserver = new ResizeObserver(() => {
    console.log('resize observer instance created.');
    initialHeroImagePosition();
  });

  if (hero.value) {
    resizeObserver.observe(hero.value);
  }
}

onMounted(() => {
  createObserver();
  createResizeObserver();
  updateScreenSize();
  initializePageContent();
  initialHeroImagePosition();
  window.addEventListener('scroll', handleScroll);
  window.addEventListener('resize', updateScreenSize);
});

onUnmounted(() => {
  if (resizeObserver && hero.value) {
    resizeObserver.unobserve(hero.value);
  }
  window.removeEventListener('scroll', handleScroll);
});

</script>
<template>
  <section id="hero" class="hero" ref="hero">
    <div class="bg-layer">
      <video autoplay muted loop class="hero-video" ref="heroVideo">
        <source src="@/assets/background-video.mp4" type="video/mp4" />
      </video>
      <div class="overlay-layer">
        <img v-if="screenSize === 'small'" src="@/assets/HomePageImages/overlay-shape-1-sm.svg" alt="shapes">
        <img v-else src="@/assets/HomePageImages/overlay-shape-1.svg" alt="shapes">
      </div>
    </div>
    <div class="hero-info" ref="heroInfo">
      <div class="content-wrapper" ref="contentWrapper">
        <h1>
          <!-- {{store.MediAssistConfigManager.DomainName}} AI <br><span>Redefining Medical Consultations</span> -->
           {{ store.MediAssistConfigManager.DomainName }} AI
        </h1>
        <div class="description">Redefining Medical Consultations</div>
        <p>Revolutionizing Healthcare with AI-Powered Assistants, Transcriptions, Diagnostics, and Smart Support.</p>
        <button @click="showScheduleDemoPopUp = true" class="btn btn-primary">
          <div class="btn-background">
            <div class="inside-text">
              Schedule a demo
            </div>
          </div>
          <div class="border-gradient-fn">
          </div>
        </button>
      </div>
    </div>
    <div class="hero-image" ref="heroImage">
      <img class="img-sm" src="@/assets/HomePageImages/hero-image-lg.jpg" alt="hero-image">
    </div>
    <div class="clip-layer">
      <img v-if="screenSize === 'small'" src="@/assets/HomePageImages/clip-shape-sm.svg" alt="clip shape">
      <img v-else src="@/assets/HomePageImages/clip-shape.svg" alt="clip shape">
    </div>
  </section>
  <section id="speciality" class="speciality" ref="speciality">
    <div class="content">
      <div class="container-top">
        <h3>Driving Innovation in Healthcare with AI </h3>
        <p>Innovative AI-driven transcriptions and diagnostics, transforming healthcare workflows daily with an AI virtual assistant in healthcare.</p>
      </div>
      <div v-if="!isHighlightsCarousel" class="container-bottom">
        <div class="card">
          <div class="card-icon">
            <img src="../assets/icons/transcription-icon.svg" alt="mic icon">
          </div>
          <div class="card-content">
            <div class="card-title">Real-Time Transcription</div>
            <div class="card-description">
                Transform speech into structured text during consultations with the help of an AI powered doctor assistant.
            </div>
          </div>
          <div class="card-footer">
            <div class="image-wrapper">
              <img src="../assets/HomePageImages/speciality-card-1.png" alt="card">
              <div class="shadow"/>
            </div>
          </div>
        </div>
        <div class="card">
          <div class="card-icon">
            <img src="../assets/icons/prescription-icon.svg" alt="mic icon">
          </div>
          <div class="card-content">
            <div class="card-title">Prescription Generator</div>
            <div class="card-description">Streamline prescription creation with a smart, automated tool, powered by an AI doctor assistant.</div>
          </div>
          <div class="card-footer">
            <div class="image-wrapper">
              <img src="../assets/HomePageImages/speciality-card-2.png" alt="prescription generator">
              <div class="shadow"/>
            </div>
          </div>
        </div>
        <div class="card">
          <div class="card-icon">
            <img src="../assets/icons/report-icon.svg" alt="mic icon">
          </div>
          <div class="card-content">
            <div class="card-title">Report Generation</div>
            <div class="card-description">
                Create detailed medical reports effortlessly with one-click, leveraging an AI Doctor  designed for healthcare.
            </div>
          </div>
          <div class="card-footer">
            <div class="image-wrapper">
              <img src="../assets/HomePageImages/speciality-card-3.png" alt="card">
              <div class="shadow"/>
            </div>
          </div>
        </div>
      </div>
      <CarouselCard v-if="isHighlightsCarousel" :content="highlightsData" :interval="5000" :indicators="indicators" :slidesPerView="slidesPerView" :type="CAROUSEL_TYPE.SPECIALITY"></CarouselCard>
    </div>
  </section>
  <section id="highlights" class="highlights" ref="highlights">
    <div class="container">
      <div class="video">
        <video autoplay muted controls loop class="hero-video">
          <source src="@/assets/highlights.mp4" type="video/mp4" />
        </video>
      </div>
    </div>
  </section>
  <section id="features" ref="features">
    <CarouselCard :content="content" :interval="5000" :indicators="indicators" :slidesPerView="slidesPerView" :type="CAROUSEL_TYPE.DEFAULT"></CarouselCard>
  </section>
  <section id="get-started" class="get-started" ref="getStarted">
    <div class="container">
      <div class="gradient-layer">
        <div class="content">
          <h3>Say hello to your AI resident.</h3>
          <p>It's like you, but less gorgeous.</p>
          <button class="btn btn-primary" @click="handleGetStarted">
            <div class="btn-background">
              <div class="inside-text">
                <img src="@/assets/icons/start-icon.svg" alt="start-icon">
                <span>Get Started</span>
              </div>
            </div>
            <div class="border-gradient-fn"></div>
          </button>
        </div>
      </div>
    </div>
  </section>
  <ScheduleDemoPopUp
      :is-visible="showScheduleDemoPopUp"
      modal-type="schedule-demo"
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
      content: '';
      position: absolute;
      z-index: 1;
      width: 100%;
      height: inherit;
      background: linear-gradient(93deg, #060815 2.2%, rgba(6, 8, 21, 0.50) 97.48%);
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
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 1.5rem;
      font-family: Inter, 'segoe ui', sans-serif;
      min-width: 21.3125rem;

      @media (min-height: 840px) {
        padding-top: 5rem;
        gap: 2.5rem;
      }

      @media (min-width: 426px) and (max-width: 744px) {
        max-width: 30rem;
      }

      @media (min-width: 745px) {
        max-width: none;
      }

      h1 {
        font-size: 2.625rem;
        font-weight: 600;
        line-height: 3.75rem;
        text-align: center;
        font-family: Inter, 'segoe ui', sans-serif;
        width: 100%;
        padding-inline: 1rem;

        @media (max-width: 480px) {
          font-size: 1.5rem;
          line-height: 2.25rem;
          color: #fff;
          text-align: center;
          font-family: Inter, 'segoe ui', sans-serif;
          font-style: normal;
          width: 18ch;
        }
      }

      div.description {
          font-size: 2.635rem;
          font-weight: 400;
          line-height: 3.75rem;
          text-align: center;

          @media (max-width: 480px) {
            font-size: 1.5rem;
            line-height: 2.25rem;
            color: #fff;
            text-align: center;
            font-family: Inter, 'segoe ui', sans-serif;
            font-style: normal;
          }

          @media (max-width: 744px) {
            font-size: 1.5rem;
            font-weight: 400;
            line-height: 2.25rem;
            text-align: center;
            font-family: Inter, 'segoe ui', sans-serif;
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
      border-radius: inherit;
      opacity: .85;
      transform: scale(1.05);
    }

    & img {
      width: 1046px;
      height: auto;
      aspect-ratio: 1.4 /1;

      @media (max-width: 480px) {
        width: 287px
      }

      @media (min-width: 481px) and (max-width: 768px) {
        width: 425px
      }

      @media (min-width: 769px) and (max-width: 1200px) {
        width: 725px
      }
    }
  }
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

      & h3 {
        text-align: center;
        font-size: 0.875rem;
        font-weight: 600;
        line-height: 1.3125rem;
        letter-spacing: 0.175rem;
        text-transform: uppercase;
        color: #0066d4;

        @media (min-width: 481px) {
          text-align: center;
          font-size: 1rem;
          font-weight: 600;
          line-height: 1.5rem;
          letter-spacing: 0.2rem;
          text-transform: uppercase;
          color: #0066d4;
        }
      }

      & p {
        color: #1C1C1C;
        text-align: center;
        font-family: inherit;
        font-size: 1.375rem;
        font-style: normal;
        font-weight: 400;
        line-height: 2.0625rem;

        @media (min-width: 481px) {
          color: #1C1C1C;
          text-align: center;
          font-family: inherit;
          font-size: 2.125rem;
          font-style: normal;
          font-weight: 400;
          line-height: 150%;
        }
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
              background: #FFF;
              box-shadow: 0 0.164875rem 0.5770625rem -0.04125rem #E5F1FF;

              @media (min-width: 481px) {
                width: inherit;
                border-radius: 0.5rem;
                background: #FFF;
                box-shadow: 0 0.164875rem 0.5770625rem -0.04125rem #E5F1FF;
              }
            }

            & .shadow {
              position: absolute;
              width: 100%;
              height: 5.125rem;
              bottom: 0;
              left: 0;
              background: linear-gradient(180deg, rgba(250, 250, 250, 0.00) 0%, #FAFAFA 75.44%, #FAFAFA 107.78%);
              z-index: 100;

              @media (min-width: 481px) {
                position: absolute;
                width: 100%;
                height: 5.125rem;
                bottom: 0;
                left: 0;
                background: linear-gradient(180deg, rgba(250, 250, 250, 0.00) 0%, #FAFAFA 75.44%, #FAFAFA 107.78%);
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
          font-family: Inter, 'segoe ui', sans-serif;
        }

        & .card-description {
          margin-top: 0.3125rem;
          color: #4F4F4F;
          font-size: 0.875rem;
          font-style: normal;
          font-weight: 400;
          line-height: 150%;
          font-family: Inter, 'segoe ui', sans-serif;
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
        content: '';
        position: absolute;
        top: 4.433rem;
        right: 2.552rem;
        width: 16.4375rem;
        height: 12rem;
        background: url('@/assets/HomePageImages/overlay-shape-2.svg') no-repeat;
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

        & h3, & p {
          max-width: 100%;

          @media (min-width: 769px) {
            max-width: 27.3125rem;
          }
        }

        & h3 {
          color: #FFF;
          font-family: Inter, 'segoe ui', sans-serif;
          font-size: 0.875rem;
          font-style: normal;
          font-weight: 600;
          line-height: 1.3125rem;
          letter-spacing: 0.175rem;

          @media (min-width: 769px) {
            font-size: 1.125rem;
            line-height: 1.6875rem;
            letter-spacing: 0.225rem;
          }
        }

        & p {
          color: #FFF;
          font-family: Inter, 'segoe ui', sans-serif;
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

    & .video {
        display: flex;
        flex-direction: column;
        width: 100%;
        height: 100%;
        justify-content: center;

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

#features {
  background: white;
  padding-inline: 1rem;

  @media (max-width: 468px) {
    padding-block-end: 1rem;
  }
}

.get-started {
  width: 100%;
  height: 23.625rem;
  background: url("@/assets/HomePageImages/get-started-background-image.png") center no-repeat;
  background-size: cover;
  position: relative;

  @media (min-width: 471px) {
    height: 31.8125rem;
  }

  & .container {
    width: inherit;
    height: inherit;

    & .gradient-layer {
      position: absolute;
      top: 0;
      left: 0;
      width: inherit;
      height: inherit;
      background: linear-gradient(266deg, rgba(52, 115, 209, 0.00) 0.98%, #12307C 98.85%);

      & .content
    {
        color: #FFF;
        width: 100%;
        height: inherit;
        display: flex;
        flex-direction: column;
        justify-content: flex-end;
        align-items: flex-start;
        max-width: 1280px;
        padding-inline: 1rem;
        padding-block: 1.5rem;

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
          font-family: Inter, 'segoe ui', sans-serif;

          @media (min-width: 481px) {
            width: fit-content;
            color: inherit;
            font-size: 2.625rem;
            font-style: normal;
            font-weight: 400;
            line-height: 4.463rem;
            font-family: Inter, 'segoe ui', sans-serif;
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