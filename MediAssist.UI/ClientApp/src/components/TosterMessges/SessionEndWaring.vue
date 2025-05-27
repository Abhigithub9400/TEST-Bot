<template>
  <div class="session-end-main" v-if="isVisible">
    <div v-if="!SessionWillEndSoon" class="session-end-pop-up">
      <div class="alert-text-container">
        <div>
          <svg
            width="40"
            height="40"
            viewBox="0 0 40 40"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <circle cx="20" cy="20" r="20" fill="#167BE8" />
            <g clip-path="url(#clip0_3442_987)">
              <path
                d="M15.375 10.625C15.7202 10.625 16 10.9048 16 11.25V12.1875H24.125V11.25C24.125 10.9048 24.4048 10.625 24.75 10.625C25.0952 10.625 25.375 10.9048 25.375 11.25V12.1875H26.625C27.6605 12.1875 28.5 13.027 28.5 14.0625V19.815C28.1105 19.5897 27.6914 19.4097 27.25 19.2823V16.875H12.875V24.6875C12.875 25.0327 13.1548 25.3125 13.5 25.3125H20.0968C20.1447 25.7459 20.2419 26.1645 20.3826 26.5625H13.5C12.4645 26.5625 11.625 25.723 11.625 24.6875V14.0625C11.625 13.027 12.4645 12.1875 13.5 12.1875H14.75V11.25C14.75 10.9048 15.0298 10.625 15.375 10.625Z"
                fill="white"
              />
              <path
                fill-rule="evenodd"
                clip-rule="evenodd"
                d="M25.6875 20C28.2763 20 30.375 22.0987 30.375 24.6875C30.375 27.2763 28.2763 29.375 25.6875 29.375C23.0987 29.375 21 27.2763 21 24.6875C21 22.0987 23.0987 20 25.6875 20ZM25.6875 26.875C25.3423 26.875 25.0625 26.5952 25.0625 26.25C25.0625 25.9048 25.3423 25.625 25.6875 25.625C26.0327 25.625 26.3125 25.9048 26.3125 26.25C26.3125 26.5952 26.0327 26.875 25.6875 26.875ZM26.3125 22.5C26.3125 22.1548 26.0327 21.875 25.6875 21.875C25.3423 21.875 25.0625 22.1548 25.0625 22.5V24.6875C25.0625 25.0327 25.3423 25.3125 25.6875 25.3125C26.0327 25.3125 26.3125 25.0327 26.3125 24.6875V22.5Z"
                fill="white"
              />
            </g>
            <defs>
              <clipPath id="clip0_3442_987">
                <rect
                  width="20"
                  height="20"
                  fill="white"
                  transform="translate(11 10)"
                />
              </clipPath>
            </defs>
          </svg>
        </div>
        <div class="button-container">
          <p class="remove-margin text-color-white">Your trial has expired!</p>
        </div>
      </div>
      <div class="subscribe-button-container">
        <button
          @click="NavigateToPricing"
          class="remove-margin subscribe-button text-color-blue"
        >
          Subscribe Now
        </button>
      </div>
      <div class="button-container" @click="closePopup">
        <button class="remove-margin close-button text-color-white">X</button>
      </div>
    </div>
    <div v-if="SessionWillEndSoon" class="session-end-pop-up minimum-height ">
      <div class="button-container">
        <p class="remove-margin text-color-white ">Session will be End Soon</p>
      </div>
      <div class="button-container" @click="closePopup">
        <button class="remove-margin close-button text-color-white">X</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, defineProps, defineEmits } from "vue";
import { useRouter } from "vue-router";

const props = defineProps({
  showSessionWillEndSoon: {
    type: Boolean,
    default: true,
  },
});
const SessionWillEndSoon = ref(props.showSessionWillEndSoon);
const isVisible = ref(true);
const router = useRouter();

const emits = defineEmits(["close"]);
const closePopup = () => emits("close");

const NavigateToPricing = () => {
  router.replace("/pricing");
};
</script>
<style scoped>
.session-end-main {
  bottom: 21px;
  display: flex;
  left: 38%;
  position: fixed;
  background-color: #007bff;
  flex-direction: row;
  flex-wrap: wrap;
  align-items: center;
  border-radius: 6px;
}
.remove-margin {
  margin: 0px !important;
}
.text-color-white {
  color: white;
}
.text-color-blue {
  color: #007bff;
}

.session-end-pop-up {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  height: auto;
  padding: 3px;
  padding-left: 15px;
  padding-right: 15px;
}
.close-button {
  background: none;
  border: none;
}
.subscribe-button-container {
  margin-left: 30px;
}
.subscribe-button {
  background-color: white;
  border: none;
  border-radius: 2px;
  padding-top: 4px;
  padding-bottom: 4px;
  padding-right: 14px;
  padding-left: 14px;
}
.button-container {
  margin-left: 10px;
}
.alert-text-container {
  display: flex;
  align-items: center;
  flex-direction: row;
}
.minimum-height{
    height: 40px;
}
</style>
