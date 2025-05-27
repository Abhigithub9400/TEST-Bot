<template>
    <div class="website resources-page-container">
        <div class="resources-banner" v-scroll-reveal>
            <div class="resources-title">
                <h1 class="h1-semibold">Resources</h1>
                <p class="lg-regular">
                    Reach out to our team for inquiries, partnership opportunities, or demo requests. We are here to assist you!
                </p>
            </div>
        </div>

        <div class="resources-section" id="resourcesSection" ref="resourcesSection">

            <div class="resource-toggle-group" style="display: none;">
                <button class="toggle-button active toggle-button-blog">Blogs</button>
                <button class="toggle-button">Case studies</button>
                <button class="toggle-button">White paper</button>
            </div>

            <div class="resources-container">
                <div class="resources-blog1" v-scroll-reveal="{ origin: 'top', distance: '50px', delay: 300 }">
                    <router-link to="/resourcesblog1" class="blog1-header-image"><img src="@/assets/BlogImages/Blog_1_Main_Image.jpg" class="blog1-main-image"/></router-link>
                    <div class="blog1-main-text">
                        <router-link to="/resourcesblog1" class="blog1-header-text" title="Transform Healthcare Documentation with AI Medical Scribe Software">
                            Transform Healthcare Documentation with AI Medical Scribe Software
                        </router-link>
                        <div class="blog1-info-subtext">
                            In the modern healthcare landscape, documentation can be overwhe...
                            <span class="blog1-navigation">
                                <router-link to="/resourcesblog1" class="blog-nav-link">Read more</router-link>
                            </span>
                        </div>
                        <div class="author-details">
                            <img src="@/assets/BlogImages/Blog1Author.jpg" alt="Author of First Blog" class="author-image">
                            <div class="author-name-blog-date">
                                <div class="author-name">Susan Elizabeth Abraham</div>
                                <div class="blog1-date">April 02, 2025</div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="resources-blog1" v-scroll-reveal="{ origin: 'top', distance: '50px', delay: 300 }">
                    <router-link to="/resourcesblog2" class="blog1-header-image"><img src="@/assets/BlogImages/Blog_2_Main_Image.jpg" class="blog1-main-image"/></router-link>
                    <div class="blog1-main-text">
                        <router-link to="/resourcesblog2" class="blog1-header-text" title="Medical Documentation with MediNoteX the Leading Medical Transcription Software">
                            Medical Documentation with MediNoteX the Leading Medical Transcription Software
                        </router-link>
                        <div class="blog1-info-subtext">
                            In today’s fast-paced healthcare environment, accurate and efficient medical...
                            <span class="blog1-navigation">
                                <router-link to="/resourcesblog2" class="blog-nav-link">Read more</router-link>
                            </span>
                        </div>
                        <div class="author-details">
                            <img src="@/assets/BlogImages/Blog2Author.jpg" alt="Author of Second Blog" class="author-image">
                            <div class="author-name-blog-date">
                                <div class="author-name">Remya Rajendran Nair</div>
                                <div class="blog1-date">April 16, 2025</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>

</script>

<style scoped>
.resources-page-container{
    overflow-x: hidden;
}

.resources-banner {
    text-align: center;
    width: 100%;
    height: 260px;
    background: linear-gradient(0deg, #050613 0%, #051B2F 100%);
    display: flex;
    align-items: center;
    justify-content: center;
}

.resources-title {
    text-align: left;
    color: #FFFFFF;
    min-width: 1280px;
    position: relative;
    top: 40px;
    align-items: flex-start;
    display: flex;
    flex-direction: column;
    margin-inline: auto;

    h1,p {
        margin: 0;
    }
}

.resources-section {
    display: flex;
    justify-content: space-between;
    padding: 40px 0px;

    @media (min-width: 481px) {
      width: 100%;
      height: inherit;
      display: flex;
      flex-direction: column;
      align-items: center;
    }

    @media (min-width: 1440px) {
      margin-inline: auto;
    }
}

.resource-toggle-group {
    display: flex;
    padding: 3.776px;
    justify-content: center;
    align-items: center;
    gap: 24px;
    border-radius: 94.407px;
    background: var(--Surface-Background-Grey, #EFEFEF);
}

.toggle-button {
  background-color: transparent;
  border: none;
  padding: 8px 16px;
  /* border-radius: 20px; */
  border-radius: 94.407px;
  font-weight: 500;
  font-size: 14px;
  cursor: pointer;
  color: #333;
  transition: background-color 0.2s ease;
}

.toggle-button-blog{
    display: flex;
    padding: 9.441px 16px;
    justify-content: center;
    align-items: center;
    gap: 3.776px;
    border-radius: 94.407px;
    background: var(--Primary, #0066D4);
}

.toggle-button:hover {
  background-color: #ddd;
}

.toggle-button.active {
    background: var(--Primary, #0066D4);
  color: white;
}


.resources-container {
    gap: 20px;
    min-width: 1280px;
    position: relative;
    margin-inline: auto;
    display: flex;
    flex-direction: row;
    align-items: flex-start;
    margin-top: 20px;
}

.resources-blog1 {
    width: 380px;
    align-items: center;
    height: 410px;
    border-radius: 16px;
    border-width: 1px;
    display: flex;
    flex-direction: column;
    background: #FFFFFF;
    border: 1px solid #E4E4E4;
    padding: 16px 0px;
}

.blog1-header-image{
    font-weight: 500;
    font-size: 32px;
    line-height: 48px;
    gap: 10px;
    display: flex;
    flex-direction: column;
    border-radius: 8px;
    padding: 0px 16px 16px 16px;
}

.blog1-main-image{
    display: flex;
    width: 344px;
    height: 195px;
    align-items: center;
    gap: 10px;
}

.blog1-main-text{
    gap: 10px;
    display: flex;
    flex-direction: column;
    padding: 0px 16px;
    /* text-align: justify; */
}

.blog1-header-text{
    line-height: 32px;
    gap: 9.74px;
    display: flex;
    flex-direction: column;
    font-weight: 500;
    letter-spacing: 0;
    font-size: 20px;
    display: -webkit-box;
    -webkit-line-clamp: 2;
    line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
    text-overflow: ellipsis;
    cursor: pointer;
    text-decoration: none;
    color: #000000;
}

.blog1-header-text:hover{
    color: #0066D4;
}

.blog1-info-subtext{
    font-weight: 400;
    font-size: 16px;
    line-height: 28px;
    letter-spacing: 0;
}

.author-details{
    width: 230PX;
    height: 46PX;
    gap: 12px;
    display: flex;
    flex-direction: row;
    justify-content: flex-start;
    align-items: center;
}

.author-image{
    width: 40px;
    height: 40px;
}

.author-name-blog-date{
    width: 140px;
    height: 46px;
}

.author-name{
    width: 200px;
    height: 28px;
    font-weight: 400;
    font-size: 16px;
    line-height: 28px;
    letter-spacing: 0%;
    color: #1C1C1C;
}

.blog1-date{
    font-weight: 400;
    font-size: 12px;
    line-height: 18px;
    letter-spacing: 0;
    color: #646363;
}

.blog1-navigation{
    font-weight: 600;
    font-size: 16px;
    line-height: 28px;
    letter-spacing: 0%;
    text-decoration: underline;
    text-decoration-style: solid;
    /* text-decoration-offset: 0%; */
    text-decoration-thickness: 0%;
}

.blog-nav-link{
    text-decoration: none;
    color: #1D0329;
    cursor: pointer;
}

</style>
