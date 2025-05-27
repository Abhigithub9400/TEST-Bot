<template>
    <div class="center-image">
      <div class="container">
        <div class="gradient-layer">
          <div class="content">
            <h3 v-scroll-reveal="{ distance: '50px', delay: 600 }">Experience {{ store.MediAssistConfigManager.DomainName }} in action.</h3> 
            <h3 v-scroll-reveal="{ distance: '50px', delay: 600 }">Book a Demo today!</h3>
            <button class="btn btn-primary" @click="handleRequestDemo('request-demo')" v-scroll-reveal="{ distance: '50px', delay: 600 }">
              <div class="btn-background">
                <div class="inside-text">
                  <span>Book a Demo</span>
                </div>
              </div>
              <div class="border-gradient-fn"></div>
            </button>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { useMyStore } from "@/store/store.ts";
  import { useRouter } from 'vue-router';
  
  const store = useMyStore();
  const router = useRouter();
  
  const handleRequestDemo = (modalType) => {
    localStorage.setItem('modalType', modalType);
    router.push('/request-demo');
  };
  </script>
  
  <style scoped>
  .book-demo {
    height: 404px;
    background: white;
    background-size: cover;
    position: relative;
  }
  
  .center-image {
    padding: 60px;
    display: flex;
    justify-content: center;
  }
  
  .container {
    width: 1270px;
    height: 310px;
    background: white url("@/assets/HomePageImages/book-demo-background-image.jpg") center no-repeat;
    background-size: cover;
    border-radius: 30px;
  }
  
  .gradient-layer {
    height: inherit;
    background: linear-gradient(266.01deg, rgba(52, 115, 209, 0) 0.98%, #0E2766 98.85%);
    border-radius: 30px;
  }
  
  .content {
    color: #FFF;
    height: inherit;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    max-width: 1280px;
    padding-inline: 5rem;
    padding-block: 3rem;
  }
  
  @media (min-width: 481px) and (max-width: 1280px) {
    .content {
      padding-inline: 3.125rem;
      padding-block: 0;
      justify-content: center;
      margin-inline: auto;
    }
  }
  
  @media (max-width: 744px) {
    .content {
      padding-inline: 1rem;
    }
  }
  
  @media (min-width: 1281px) {
    .content {
      margin-inline: auto;
    }
  }
  
  h3 {
    color: inherit;
    font-size: 1.5rem;
    font-style: normal;
    font-weight: 400;
    line-height: 150%;
    margin: 0px;
    width: 14ch;
    font-family: Inter, 'segoe ui', sans-serif;
  }
  
  @media (min-width: 481px) {
    h3 {
      width: fit-content;
      color: inherit;
      font-size: 2.625rem;
      font-style: normal;
      font-weight: 400;
      line-height: 4.463rem;
      font-family: Inter, 'segoe ui', sans-serif;
    }
  }
  
  .btn {
    margin-top: 1.5rem;
    width: 150px;
    cursor: pointer;
  }
  </style>
  