<script setup>
import { ref, onBeforeUnmount, defineProps, computed, onMounted } from 'vue';
import CarouselIndicators from '@/components/carousel/CarouselIndicators.vue';
import CarouselItem from '@/components/carousel/CarouselItem.vue';
import { useMyStore } from '@/store/store.ts';

const props = defineProps({
  content: {
    type: Array,
    required: true,
  },
  controls: {
    type: Boolean,
    default: true,
  },
  indicators: {
    type: Boolean,
    default: false,
  },
  interval: {
    type: Number,
    default: 5000,
  },
  slidesPerView: {
    type: Number,
    default: 2,
  },
  type: {
    type: Number,
    default: 1,
  }
});

const store = useMyStore();

const slides = computed(() => {
  const slidesPerView = props.slidesPerView;
  if (slidesPerView < 1) {
    return [];
  }

  const finalArray = [];
  let bufferArray = [];

  props.content.forEach((slide, index) => {
    bufferArray.push(slide);
    if (bufferArray.length === slidesPerView || index === props.content.length - 1) {
      finalArray.push(bufferArray);
      bufferArray = [];
    }
  });

  return finalArray;
})

const currentSlide = ref(0);
const slideInterval = ref(null);
const direction = ref('right');

const setCurrentSlide = (index) => {
  currentSlide.value = index;
};

const stopSlideTimer = () => {
  clearInterval(slideInterval.value);
};

const startSlideTimer = () => {
  stopSlideTimer();
  slideInterval.value = setInterval(() => {
    _next();
  }, props.interval);
};

const prev = (step = -1) => {
  const index = currentSlide.value > 0 ? currentSlide.value + step : slides.value.length - 1;
  setCurrentSlide(index);
  direction.value = 'left';
  startSlideTimer();
};

const _next = (step = 1) => {
  const index =
      currentSlide.value < slides.value.length - 1
          ? currentSlide.value + step
          : 0;
  setCurrentSlide(index);
  direction.value = "right";
}

const next = (step = 1) => {
  _next(step);
  startSlideTimer();
};

const switchSlide = (index) => {
  const step = index - currentSlide.value;
  if (step > 0) {
    next(step);
  } else {
    prev(step);
  }
};

onMounted(() => {
  startSlideTimer();
});

onBeforeUnmount(() => {
  stopSlideTimer();
});
</script>

<template>
  <div class="features">
    <div class="header" v-if="type === 1">
      <div class="section-left">
        <p>WHY CHOOSE US?</p>
        <p>Unmatched accuracy, reliability, and adaptability make {{store.MediAssistConfigManager.DomainName}} AI ideal for modern healthcare.</p>
      </div>
      <div class="section-right">
        <div class="container">
          <button class="btn-left" @click="prev(-1)">
            <img src="../../assets/icons/left-arrow.svg" alt="left arrow icon">
          </button>
          <span class="count">{{ (currentSlide + 1) + "/" + slides.length }}</span>
          <button class="btn-right" @click="next(1)">
            <img src="../../assets/icons/right-arrow.svg" alt="right arrow icon">
          </button>
        </div>
      </div>
    </div>
    <div class="content">
      <div class="carousel">
        <div class="carousel-inner" >
          <CarouselItem
              v-for="(slide, index) in slides"
              :slide="slide"
              :key="`item-${index}`"
              :current-slide="currentSlide"
              :index="index"
              :direction="direction"
              :type="type"
          ></CarouselItem>
      </div>
    </div>
  </div>
    <CarouselIndicators
        v-if="indicators"
        :total="slides.length"
        :current-index="currentSlide"
        @switch="switchSlide($event)"
    ></CarouselIndicators>
  </div>
</template>

<style scoped>
.features {
  width: 100%;
  height: auto;
  display: flex;
  flex-direction: column;
  padding: 2rem 0;
  gap: 2.296rem;
  background: white;
  max-width: 80rem;
  margin-inline: auto;
  position: relative;

  @media (min-width: 769px) {
    padding: 2rem 1rem;
    width: auto;
  }

  @media (min-width: 1440px) {
    padding-inline: 0;
  }

  & .header {
    display: flex;
    height: auto;

    & .section-left {
      display: flex;
      flex-direction: column;
      justify-content: flex-start;
      align-items: flex-start;
      flex-grow: 1;
      gap: 0.875rem;

      & p {
        color: #0066D4;
        font-family: Inter, 'segoe ui', sans-serif;
        font-size: 0.875rem;
        font-style: normal;
        font-weight: 600;
        line-height: 150%;
        letter-spacing: 0.175rem;
        text-transform: uppercase;
        margin: 0;

        @media (min-width: 481px) {
          font-size: 1rem;
          letter-spacing: 0.2105rem;
        }
      }
      & p:not(:first-child) {
        color: #1C1C1C;
        font-family: Inter, 'segoe ui', sans-serif;
        font-size: 1.375rem;
        font-style: normal;
        font-weight: 400;
        line-height: 170%;
        text-transform: none;

        @media (min-width: 481px) {
          font-size: 2.125rem;
        }
      }
    }

    & .section-right {
      display: flex;
      justify-content: flex-end;
      align-items: flex-end;
      padding-block-end: 1.25rem;

      @media (max-width: 768px) {
        display: none;
      }

      & .container {
        width: fit-content;
        height: fit-content;
        display: flex;

        & .btn-left, & .btn-right {
          display: flex;
          padding: 0.625rem;
          border-radius: 0.0282rem;
          border: none;

          &:hover {
            cursor: pointer;
          }

          & img {
            border-radius: inherit;
          }
        }

        & .count {
          display: flex;
          align-items: center;
          justify-content: center;
          width: 3.487rem;
        }
      }
    }
  }

  & .content {
    flex-grow: 1;
    height: 30.25rem;

    @media (min-width: 481px) and (max-width: 768px) {
      height: 39.25rem;
    }

    & .carousel {
      height: 100%;
      display: flex;
      justify-content: flex-start;

      & .carousel-inner {
        position: relative;
        width: 100%;
        height: inherit;
        overflow: hidden;
      }
    }
  }
}
</style>
