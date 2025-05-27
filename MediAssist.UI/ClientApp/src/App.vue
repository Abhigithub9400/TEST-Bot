<template>
  <div id="app">
    <router-view></router-view>
    <div v-if="isOffline" class="offline-overlay"></div>
    <OfflineError :isOffline="isOffline"/>
  </div>
</template>

<script>
import { ref, onMounted, onUnmounted, watch } from "vue";
import OfflineError from "./components/TosterMessges/OfflineWarning.vue";
import { useMyStore } from '@/store/store.ts';

export default {
  name: "App",
  components: {
    OfflineError,
  },
  setup() {
    const isOffline = ref(false);

    const updateNetworkStatus = () => {
      isOffline.value = !navigator.onLine;
    };

    const store = useMyStore();

    onMounted(async () => {
      updateNetworkStatus(); 
      await store.fetchGlobalConfigurations();
      window.addEventListener("online", updateNetworkStatus);
      window.addEventListener("offline", updateNetworkStatus);
    });

    onUnmounted(() => {
      window.removeEventListener("online", updateNetworkStatus);
      window.removeEventListener("offline", updateNetworkStatus);
    });

    watch(isOffline, (newVal) => {
      if (newVal) {
        document.body.classList.add("no-scroll");
      } else {
        document.body.classList.remove("no-scroll");
      }
    });

    return {
      isOffline,
    };
  },
};
</script>

<style>
@import url("https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap");

body {
  font-family: "Inter", sans-serif;
  font-style: normal;
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  transition: all 0.3s ease;
}

body.no-scroll {
  overflow: hidden;
}

#app {
  position: relative;
}
.offline-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5); 
  z-index: 9999; 
  pointer-events: all; 
}
</style>
