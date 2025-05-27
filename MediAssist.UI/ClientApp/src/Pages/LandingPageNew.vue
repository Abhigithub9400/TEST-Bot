<script setup>
import AppLogo from '@/components/AppLogo.vue';
import { ref, onMounted, onUpdated } from 'vue';
import { useRoute } from 'vue-router';
import { useRouter } from 'vue-router';
import { useMyStore } from '@/store/store.ts';
import ShowContactUsPopUp from '@/Pages/modals/GenericModal.vue';
import DeleteAccountSuccessAlert from "@/components/alert/DeleteSuccessPopUp.vue";
import { useScrollHeader } from '@/composables/useHeaderObserver';

const { isScrolled } = useScrollHeader();

const isShowMenu = ref(false);
const header = ref(null);
const showContactUsPopUp = ref(false);
const showDeleteSuccessPopup = ref(false);
const deleteAccountSuccessAlertMessage = ref("");

const router = useRouter();
const route = useRoute();
const store = useMyStore();

const IntializePage = () => {
  deleteAccountSuccessAlertMessage.value = `Your account has been successfully deleted. Thank you for using ${store.MediAssistConfigManager.DomainName} AI.`;
};
const hamburgerClickHandler = () => {
  isShowMenu.value = !isShowMenu.value;
};

const handleContactUsWithMenuClose = (modalType) => {
  isShowMenu.value = false;
  localStorage.setItem('modalType', modalType);
  router.push('/contact-us');
};

const handleContactUs = (modalType) => {
  localStorage.setItem('modalType', modalType);
  router.push('/contact-us');
};

const reloadPage = () => {
  window.location.reload();
};


onMounted(() => {
  IntializePage();
  if (sessionStorage.getItem('showDeleteSuccessPopup') === 'true') {
    showDeleteSuccessPopup.value = true;
  }
})

const successCancel = async () => {
    showDeleteSuccessPopup.value = false;
}


onUpdated(() => {
  const hamburgerNavManu = document.querySelector("#ham-nav-menu");
  const signupBottonNavMenu = document.querySelector("#signup-button");
  if (hamburgerNavManu) {
    if (route.path === '/chooseyourplan' || route.path === '/terms-and-conditions' || route.path === '/privacy-policy' || route.path === '/license-agreement' || route.path === '/about-us' || route.path === '/resources' || route.path === '/resourcesblog1' || route.path === '/resourcesblog2') {
      hamburgerNavManu.style.color = "#0A2144";
      if(signupBottonNavMenu){
        signupBottonNavMenu.style.color = "#0A2144";
      }
    }
  }
});
</script>
<template>
  <div id="landing-page" class="website landing-page">
    <div class="container">
      <header class="header main-header" ref="header" :class="{ scrolled: isScrolled }">
        <div class="header-wrapper" v-scroll-reveal>
          <div class="logo" @click="reloadPage">
            <AppLogo />
          </div>
          <nav class="nav-menu">
            <router-link to="/" class="nav-link">Home</router-link>
            <router-link to="/about-us" class="nav-link">About Us</router-link>
            <router-link to="/features" class="nav-link">Features</router-link>
            <router-link to="/chooseyourplan" class="nav-link">Pricing</router-link>
            <router-link to="/resources" class="nav-link">Resources</router-link>
            <router-link to="/contact-us" class="nav-link">Contact Us</router-link>
          </nav>
          <div class="buttons">
            <router-link to="/login" class="login btn btn-primary">
              <div class="btn-background">
                <div class="inside-text">
                  Login
                </div>
              </div>
              <div class="border-gradient-fn"></div>
            </router-link>
             <router-link to="/signup" class="signup btn btn-secondary" style="display: none;">Signup</router-link> 
          </div>
          <button @click="hamburgerClickHandler" class="hamburger" id="hamburger">
            <svg viewBox="0 0 32 32" fill="white" xmlns="http://www.w3.org/2000/svg">
              <path d="M28 24H4V21.3333H28V24ZM28 17.3333H4V14.6667H28V17.3333ZM28 10.6667H4V8H28V10.6667Z"/>
            </svg>
          </button>
        </div>
        <div class="header-responsive" v-if="isShowMenu">
          <nav class="nav-menu" id="ham-nav-menu">
            <router-link @click="isShowMenu = false" to="/" class="nav-link">Home</router-link>
            <router-link @click="isShowMenu = false" to="/#speciality" class="nav-link">About Us</router-link>
            <router-link @click="isShowMenu = false" to="/#features" class="nav-link">Features</router-link>
            <router-link @click="isShowMenu = false" to="/chooseyourplan" class="nav-link">Pricing</router-link>
            <router-link @click="isShowMenu = false" to="/resources" class="nav-link">Resources</router-link>
            <router-link @click.prevent="handleContactUsWithMenuClose('contact-us-enquiry')" to="/contact-us" class="nav-link">Contact Us</router-link>
          </nav>
          <div class="buttons">
            <router-link @click="isShowMenu = false" to="/login" class="btn btn-primary">
              <div class="btn-background">
                <div class="inside-text">
                  Login
                </div>
              </div>
              <div class="border-gradient-fn"></div>
            </router-link>
            <router-link @click="isShowMenu = false" to="/signup" class="btn btn-secondary" id="signup-button">Signup</router-link>
          </div>
        </div>
      </header>
      <RouterView></RouterView>
      <footer class="footer">
        <div class="container">
          <div class="content">
            <div class="left-section">
              <div class="logo" @click="reloadPage">
                <AppLogo />
              </div>
              <div class="powered-by sm-regular">
                <p>powered by</p>
                <img src="@/assets/logos/pits-logo.png" class="pits-logo">
              </div>
            </div>
            <div class="right-section">
              <ul>
                <RouterLink to="/about-us" class="lg-regular">About Us</RouterLink>
                <RouterLink to="/contact-us" class="lg-regular" @click.prevent="handleContactUs('contact-us-enquiry')">Contact Us</RouterLink>
              </ul>
              <ul>
                <RouterLink to="/terms-and-conditions" class="lg-regular">Terms and conditions</RouterLink>
                <RouterLink to="/privacy-policy" class="lg-regular">Privacy policy</RouterLink>
                <RouterLink to="/license-agreement" class="lg-regular">License agreement</RouterLink>
              </ul>
            </div>
          </div>
          <div class="copyright">
            <hr>
            <p class="sm-regular">Copyright © {{store.MediAssistConfigManager.DomainName}}. All rights reserved.</p>
          </div>
        </div>
      </footer>
    </div>
    <ShowContactUsPopUp
        v-if="showContactUsPopUp"
        :is-visible="showContactUsPopUp"
        title="Contact Us"
        description="Please fill the details for contact us"
        submitButtonText="Submit"
        modal-type="contact-us-enquiry"
        @close="showContactUsPopUp = false"
    />
    <DeleteAccountSuccessAlert
        v-if="showDeleteSuccessPopup"
        :isVisible="showDeleteSuccessPopup"
        title="Deletion Successful"
        :message = "deleteAccountSuccessAlertMessage"
        @cancel="successCancel"
        />
  </div>
</template>
<style scoped lang="css">
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}
.landing-page {
  position: relative;
  width: 100%;

    .container {
      min-width: 100%;
      height: fit-content;

      .header {
        position: fixed;
        top: 0;
        background: transparent;
        color: white;
        width: 100%;
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        align-items: center;
        height: 5rem;
        padding-inline: 1rem;
        z-index: 99999;
        transition: background-color 0.3s ease-in-out;
        padding-block: 0.5rem;

        @media (max-width: 768px) {
          display: flex;
          flex-direction: column;
          height: fit-content;
        }
        
        & .header-wrapper {
          max-width: 1280px;
          flex-grow: 1;
          margin-inline: auto;
          display: flex;
          flex-direction: row;
          justify-content: space-between;
          align-items: center;
          width: 100%;

          .logo {
            display: flex;
            align-items: center;
            cursor: pointer;
            width: fit-content;
          }

          .nav-menu {
            display: none;

            @media (min-width: 769px) {
              display: flex;
              gap: 3rem;
              flex-grow: 1;
              justify-content: center;
              font-size: 1rem;
              font-style: normal;
              font-weight: 500;
            }

            .nav-link {
              text-decoration: none;
              color: inherit;
              font-size: 18px;
              font-weight: 400;
              position: relative;
              font-style: normal;
              transition: color 0.s ease-out;
            }

            .nav-link:hover {
              transform: translateY(-1px);
              transition: transform 0.15s ease-out;
            }

            .nav-link.router-link-exact-active {
              font-weight: 600 !important;
            }
            
            .nav-link.router-link-exact-active::after {
              content: "";
              display: block;
              width: 100%;
              height: 2px;
              background-color: #0066D4;
              position: absolute;
              bottom: -5px;
              left: 0;
              transition: width 0.3s ease-in-out;
              right: 1px;
            }
          }

          .buttons {
            display: none;

            @media(min-width: 769px) {
              display: flex;
              gap: 1.5rem;
            }
          }

          .hamburger {
            width: 2rem;
            height: 2rem;
            background-color: transparent;
            border: none;

            @media(min-width: 769px) {
              display: none;
            }
          }
        }

        & .header-responsive {
          display: flex;
          flex-direction: column;
          gap: 1rem;
          width: 100%;
          justify-content: flex-start;
          align-items: center;
          height: 100vh;
          transition: height 0.5s ease-in-out;

          & .nav-menu {
            display: flex;
            flex-direction: column;
            color: white;

            & .nav-link {
              text-decoration: none;
              color: inherit;
              font-size: 1rem;
              padding: 1rem;
            }
          }

          & .buttons {
            display: flex;
            justify-content: center;
            gap: 1rem;
          }
        }
      }
      .header.main-header {
        background: transparent;
        color: white;
        transition: background-color 0.3s ease-in-out, color 0.3s ease-in-out;
      }

.header.main-header.scrolled {
  background-color: white ;
  color: #163666 ;
}

.header.main-header.scrolled .nav-link {
  color: #163666 ;
}

      .footer {
        width: 100%;
        height: fit-content;
        background: white;

        & .container {
          width: inherit;
          height: inherit;
          padding-block: 2rem;
          padding-block-end: 1rem;

          & .content
        {
            height: auto;
            width: auto;
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            max-width: 1280px;
            margin-inline: auto;
            padding-bottom: 1rem;
            padding-inline: 1rem;

            @media (min-width: 481px) and (max-width: 1280px) {
              flex-direction: row;
            }
            
            @media (min-width: 1281px) {
              flex-direction: row;
            }

        & .left-section {
              height: inherit;
              width: auto;
              flex-grow: 1;
            }
            
            & .powered-by{
              margin-top: 10px;
              & p {
                text-transform: capitalize;
                color: #787878;
              }
              & .pits-logo{
                margin-top: 7px;
                width: 44px;
                height: 44px;
              }
            }
            
            & .right-section {
              height: inherit;
              display: flex;
              flex-direction: row;
              align-items: center;
              gap: 7.125rem;
              padding-top: 1.328rem;

              @media (min-width: 481px) {
                gap: 5.75rem;
              }

              & ul {
                align-self: flex-start;
                list-style: none;
                display: flex;
                flex-direction: column;
                gap: 0.65rem;

                & a, & li {
                  color: #050505;
                  text-transform: capitalize;
                  text-decoration: none;
                  cursor: pointer;

                  @media (max-width: 481px) {
                    font-size: 1rem;
                  }
                }
              }
            }
          }

          & .copyright {
            width: inherit;
            display: flex;
            flex-direction: column;
            gap: 1rem;

            @media (max-width: 744px) {
              padding-inline: 1rem;
            }

            @media (min-width: 745px) {
              padding-inline: 3.125rem;
            }

            @media (min-width: 1281px) {
              padding-inline: 0;
              max-width: 1280px;
              margin-inline: auto;
            }

            & hr {
              height: 0.03125rem;
              background-color: rgba(10, 33, 68, 0.15);
              border: none;

              @media (max-width: 480px) {
                position: relative;
                left: 0;
                right: 0;
              }
            }

            & p {
              color: #8B8B8B;
              align-self: center;
              width: 100%;
              padding-inline: 1rem;
              text-align: center;

              @media (min-width: 481px) {
                width: auto;
                padding-inline: 0;
              }
            }
          }
        }
      }
    }
}
</style>