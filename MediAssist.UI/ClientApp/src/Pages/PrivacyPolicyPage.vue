<template>
    <div class="container">
      <header class="header-container">
        <div class="logo">
          <AppLogo />
        </div>
      </header>
  
      <main class="content">
        <section class="privacy-policy">
          <h2>Privacy Policy</h2>
          <p>
            At {{ store.MediAssistConfigManager.DomainName }}, we are committed to protecting your privacy. Here’s how we handle your data:
          </p>
          <ul>
            <li>We collect personal information to provide and improve our services.</li>
            <li>Your data is stored securely and is only accessible to authorized personnel.</li>
            <li>We do not share your information with third parties without your consent.</li>
            <li>You have the right to access, correct, or delete your personal information.</li>
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
  .privacy-policy h2 {
    font-size: 24px;
    color: #333;
  }
  
  .privacy-policy ul {
    list-style: disc;
    margin-left: 20px;
  }
  </style>
  