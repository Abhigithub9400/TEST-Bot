<template>
    <div class="container">
      <header class="header-container">
        <div class="logo">
            <AppLogo />
        </div>
      </header>
  
      <main class="content">
        <section class="guidelines">
          <h2>Terms & Conditions</h2>
          <p>
            Welcome to {{store.MediAssistConfigManager.DomainName}}! Please follow these guidelines to ensure a smooth experience:
          </p>
          <ul>
            <li>Ensure that all personal information is accurate and up-to-date.</li>
            <li>Adhere to the terms and conditions outlined in our policy.</li>
            <li>Respect the privacy of other users and their information.</li>
            <li>Report any issues or concerns to our support team promptly.</li>
          </ul>
        </section>
      </main>
    </div>
  </template>
  
  <script setup>
  import { useMyStore } from '@/store/store.ts';
  import AppLogo from '@/components/AppLogo.vue';

  const store = useMyStore();
  </script>
  
  <style scoped>
  @import url('https://fonts.googleapis.com/css2?family=Karma:wght@400;500&display=swap');
  
  .container {
    display: flex;
    flex-direction: column;
    height: 100vh;
  }
  
  .header-container {
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: #f8f9fa;
    padding: 20px;
  }
  
  .logo {
    display: flex;
    align-items: center;
    cursor: pointer;
  }
  
  .logo h3 {
    font-size: 26px;
    color: #0066D4;
    margin: 0;
  }
  
  .icon {
    width: 35px;
    height: 40px;
    margin-right: 10px;
  }
  
  .content {
    flex: 1;
    padding: 20px;
    max-width: 800px;
    margin: 0 auto;
  }
  
  .guidelines h2 {
    font-size: 24px;
    color: #333;
  }
  
  .guidelines ul {
    list-style: disc;
    margin-left: 20px;
  }
  </style>
  