<template>
  <div class="accordion">
    <div class="div-wrapper">
      <h3 class="text-wrapper h3-semibold">Frequently Asked Questions</h3>
    </div>

    <div class="frame-wrapper">
      <div class="div">
        <div class="div-2">
          <div
            v-for="tab in props.tabs"
            :key="tab.value"
            class="frame-wrapper-2"
            :class="{ active: activeTab === tab.value }"
            v-scroll-reveal="{ origin: 'bottom', distance: '50px', delay: 600 }"
          >
            <div class="div-2">
              <div class="div-3" @click="toggleTab(tab.value)">
                <h6 class="text-wrapper-2 h6-medium">
                  {{ tab.title }}
                </h6>
                <div class="outline-arrows-alt">
                  <svg width="16" height="16" viewBox="0 0 16 16" xmlns="http://www.w3.org/2000/svg">
                    <g :transform="activeTab === tab.value ? 'rotate(180 8 8)' : ''">
                      <path
                        fill-rule="evenodd"
                        clip-rule="evenodd"
                        d="M0.430571 1.1418C0.700138 0.827308 1.17361 0.790887 1.48811 1.06045L8.00001 6.64209L14.5119 1.06045C14.8264 0.790888 15.2999 0.827309 15.5695 1.1418C15.839 1.4563 15.8026 1.92977 15.4881 2.19934L8.48811 8.19934C8.20724 8.44008 7.79279 8.44008 7.51192 8.19934L0.51192 2.19934C0.197426 1.92977 0.161005 1.4563 0.430571 1.1418Z"
                        fill="#333333"
                      />
                    </g>
                  </svg>
                </div>
              </div>

              <div class="div-wrapper-2" v-if="activeTab === tab.value">
                <p class="p lg-regular">
                  {{ tab.content }}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, defineProps, watch } from "vue";

const props = defineProps({
  tabs: {
    type: Array,
    required: true,
  },
});

const activeTab = ref(null);

watch(
  () => props.tabs,
  (newTabs) => {
    if (newTabs.length > 0 && !activeTab.value) {
      activeTab.value = newTabs[0].value;
    }
  },
  { immediate: true }
);

const toggleTab = (value) => {
  if (activeTab.value === value) return;

  activeTab.value = value;
};
</script>

<style>
.accordion {
  padding: 80px !important;
  align-items: center;
  display: flex;
  flex-direction: column;
  justify-content: center;
  position: relative;
}

.accordion .div-wrapper {
  align-items: center;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 14px;
  position: relative;
  width: 100%;
}

.accordion .text-wrapper {
  align-self: stretch;
  color: #1c1c1c;
  margin-top: -1px;
  position: relative;
  text-align: center;
}

.accordion .frame-wrapper {
  align-items: flex-start;
  display: flex;
  flex: 0 0 auto;
  gap: 80px;
  position: relative;
  width: 904px;
}

.accordion .div {
  align-items: flex-start;
  display: flex;
  flex: 1;
  flex-direction: column;
  flex-grow: 1;
  gap: 32px;
  position: relative;
}

.accordion .div-2 {
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 16px;
  position: relative;
  width: 100%;
}

.accordion .frame-wrapper-2 {
  align-items: flex-start;
  align-self: stretch;
  background-color: #ffffff;
  border: 1px solid;
  border-color: #ededed;
  border-radius: 17px;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 10px;
  padding: 24px;
  position: relative;
  width: 100%;
  transition: all 0.3s ease;
}

.accordion .div-3 {
  align-items: center;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  justify-content: space-between;
  position: relative;
  width: 100%;
  cursor: pointer;
}

.accordion .text-wrapper-2 {
  color: #1c1c1c;
  margin-top: -1px;
  position: relative;
  white-space: nowrap;
  width: fit-content;
}

.accordion .outline-arrows-alt {
  height: 24px !important;
  position: relative !important;
  width: 24px !important;
  transition: transform 0.3s ease;
}

.accordion .div-wrapper-2 {
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  gap: 16px;
  position: relative;
  width: 100%;
  overflow: hidden;
  transition: all 0.3s ease;
}

.accordion .p {
  color: #5f5f5f;
  flex: 1;
  margin: 0;
  position: relative;
}

.accordion .frame-wrapper-2:hover {
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

.accordion .frame-wrapper-2.active {
  border-color: #d8d8d8;
}
</style>
