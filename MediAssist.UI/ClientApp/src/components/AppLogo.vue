<template>
  <div class="website logo" @click="goHome">
    <img src="@/assets/logos/medi-assist-logo.svg" alt="MediAssist" class="icon" />
    <h4 class="h4-bold" id="branding">{{store.MediAssistConfigManager.DomainName}}</h4>
  </div>
</template>

<script setup>
import { useRouter, useRoute } from 'vue-router';
import {defineProps, onMounted} from 'vue';
import { useMyStore } from '@/store/store.ts';

const props = defineProps({
  fromDashboard : {
    type: Boolean,
    default: false,
  }
})

const store = useMyStore();
const router = useRouter();
const route = useRoute();

let branding;

onMounted(() => {  
  branding = document.querySelector("#branding");  
  if (route.path === '/dashboard' && branding) {    
    branding.style.color = 'white';  
  }
});

const goHome = () => {
  if (props.fromDashboard ) {
    router.push('/dashboard');
  } else {
    router.push('/');
  }
};
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Karma:wght@400;500&display=swap');

.logo {
  display: flex;
  align-items: center;
  cursor: pointer;
  text-decoration: none;
  transition: transform 0.15s ease-out;
  gap: 0.5rem;
}

.logo:hover {
  transform: translateY(-1px);
  transition: transform 0.15s ease-out;
}

.logo h4 {
    width: auto;
    height: auto;
    text-align: left;
    color: #163666;
}

.icon {
  width: 43.71px;
  height: 55.52px;
  gap: 0;
}
</style>