<template>
    <div class="hero-banner" v-scroll-reveal>
      <div class="hero-banner-title">
        <h1 class="title-text h1-semibold">{{ title }}</h1>
        <p class="description lg-regular" v-html="description"></p>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: "HeroBanner",
    props: {
      title: {
        type: String,
        required: true,
      },
      description: {
        type: String,
        required: true,
      },
    },
  };
  </script>
  
  <style scoped>
  .hero-banner {
    text-align: center;
    width: 100%;
    height: 350px;
    background: linear-gradient(0deg, #050613 0%, #051B2F 100%);
    display: flex;
    align-items: center;
    justify-content: center;
  }
  
  .hero-banner-title {
    text-align: left;
    color: #FFFFFF;
    min-width: 1280px;
    position: relative;
    top: 40px;
    align-items: flex-start;
    display: flex;
    flex-direction: column;
    margin-inline: auto;
  }
  
  .title-text{
    margin: 0;
  }

  .description {
    width: 800px;
  }
  </style>
  