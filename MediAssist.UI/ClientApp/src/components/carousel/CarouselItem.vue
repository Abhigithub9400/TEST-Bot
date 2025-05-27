<script setup>
import { computed, defineProps, defineEmits } from 'vue';
defineEmits(['mouseenter', 'mouseout']);

const props = defineProps(['slide', 'currentSlide', 'index', 'direction', 'screenSize', 'type']);

const transitionEffect = computed(() => {
  return props.direction === 'right' ? 'slide-out' : 'slide-in';
});
</script>
<template>
  <transition v-if="type === 1" :name="transitionEffect">
    <div class="carousel-item" :class="screenSize" v-show="currentSlide === index" @mouseenter="$emit('mouseenter')" @mouseout="$emit('mouseout')">
      <div class="content">
        <img :src="slide[0].image" alt="carousel item " >
        <div class="item-details">
          <p>{{ slide[0].title }}</p>
          <p>{{ slide[0].description }}</p>
        </div>
      </div>
      <div v-if="slide.length === 2" class="content">
        <img :src="slide[1].image" alt="carousel item ">
        <div class="item-details">
          <p>{{ slide[1].title }}</p>
          <p>{{ slide[1].description }}</p>
        </div>
      </div>
    </div>
  </transition>
  <transition v-if="type === 2" :name="transitionEffect">
    <div class="carousel-item" :class="screenSize" v-show="currentSlide === index" @mouseenter="$emit('mouseenter')" @mouseout="$emit('mouseout')">
      <div class="card">
        <div class="card-icon">
          <img :src="slide[0].icon" alt="mic icon">
        </div>
        <div class="card-content">
          <div class="card-title">{{ slide[0].title }}</div>
          <div class="card-description">{{ slide[0].description }}</div>
        </div>
        <div class="card-footer">
          <div class="image-wrapper">
            <img :src="slide[0].image" alt="card">
            <div class="shadow"/>
          </div>
        </div>
      </div>
    </div>
  </transition>
</template>
<style scoped>
.carousel-item {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  display: flex;
  min-width: 50%;
  transition: transform 1s ease-in-out;
  gap: 1.5rem;

  & .content {
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: flex-start;
    flex-grow: 1;
    position: relative;
    margin-bottom: 2.875rem;

    @media (min-width: 481px) {
      margin-bottom: 0;
      width: 50%;
      justify-content: center;
    }

    & img {
      width: 100%;
      height: 90%;
      object-fit: cover;
      border-radius: 0.625rem;

      @media (min-width: 481px) {
        border-radius: 1rem;
      }
    }

    @media (min-width: 481px) {
      flex-direction: row;
    }

    & .item-details {
      background-color: white;
      padding-top: 1rem;
      width: 100%;
      display: flex;
      flex-direction: column;
      gap: 0.3125rem;

      @media (min-width: 481px) {
        display: block;
        position: absolute;
        bottom: 0;
        left: 0;
        width: 75%;
        padding: 1.5rem 2.5rem 1.5rem 1.5rem;
        border-top-right-radius: 1rem;
      }

      & p {
        color: #5A5A5A;
        font-family: Inter, sans-serif;
        font-size: 0.875rem;
        font-style: normal;
        font-weight: 400;
        line-height: 1.3125rem;
        margin: 0;

        @media (min-width: 481px) {
          font-size: 1rem;
          line-height: 1.5625rem;
        }
      }

      & :first-child {
        color: #1C1C1C;
        font-family: 'Inter', sans-serif;
        font-size: 1.25rem;
        font-style: normal;
        font-weight: 400;
        line-height: 1.875rem;
        margin: 0;
        padding-bottom: 0;

        @media (min-width: 481px) {
          font-size: 1.625rem;
          line-height: 2.4375rem;
        }
      }
    }
  }

  & .card {
    display: flex;
    flex-direction: column;
    align-items: start;
    border-radius: 0.8125rem;
    background-color: #fafafa;
    padding: 1.5rem;
    width: 100%;
    height: fit-content;
    position: relative;

    @media (min-width: 481px) and (max-width: 768px){
      width: 85%;
      height: fit-content;
      margin-inline: auto;
      flex-grow: 1;
    }

    @media (min-width: 769px) {
      width: 33%;
      height: fit-content;
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

        @media (min-width: 481px) and (max-width: 768px) {
          width: inherit;
          height: fit-content;
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
      font-size: 1.125rem;
      font-style: normal;
      font-weight: 400;
      line-height: 150%;
      font-family: Inter, 'segoe ui', sans-serif;
    }

    & .card-description {
      margin-top: .75rem;
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

.slide-in-enter-active,
.slide-in-leave-active,
.slide-out-enter-active,
.slide-out-leave-active {
  transition: all 1s ease-in-out;
}

.slide-in-enter-from {
  transform: translateX(-100%);
  padding-inline: 1.5rem;
}

.slide-in-leave-to {
  transform: translateX(100%);
  padding-inline: 0;
}

.slide-out-enter-from {
  transform: translateX(100%);
  padding-inline: 1.5rem;
}

.slide-out-leave-to {
  transform: translateX(-100%);
  padding-inline: 0;
}
</style>
