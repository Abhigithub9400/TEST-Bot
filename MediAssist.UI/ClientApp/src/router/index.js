import { createRouter, createWebHistory } from "vue-router";
import LandingPageNew from "../Pages/LandingPageNew.vue";
import LoginPage from "../Pages/LoginPage.vue";
import SignUpPage from "../Pages/SignUpPage.vue";
import DashBoardPage from "../Pages/DashBoardPage.vue";
import ConversationPage from "../Pages/ConversationPage.vue";
import ResetPasswordPage from "../Pages/modals/ResetPasswordPage.vue";
import ProfileManagement from '../Pages/ProfileManagement.vue';
import SubscriptionPage from '../Pages/SubscriptionPage.vue';
import PlanDetailsPage from '../Pages/PlanDetailsPage.vue';
import SettingsPage from '../Pages/SettingsPage.vue';
import AppLogo from "../components/AppLogo.vue";
import EmailHeaderLayOut from "../components/EmailHeaderLayOut.vue";
import HomeView from "@/views/HomeViewNew.vue";
import LicenseAgreementView from "@/views/LicenseAgreementView.vue";
import PrivacyPolicyView from "@/views/PrivacyPolicyView.vue";
import TermsAndConditionsView from "@/views/TermsAndConditionsView.vue";
import ErrorPage from "@/Pages/ErrorPage.vue";
import AboutUsView from "@/views/AboutUsView.vue";
import ContactUsView from "@/views/ContactUsView.vue";
import RequestDemoView from "@/views/RequestDemoView.vue";
import FeaturesPage from '@/Pages/FeaturesPage.vue';
import ResourcesView from "@/views/ResourcesView.vue";
import BlogPage1 from "@/Pages/blogs/BlogPage1.vue";
import BlogPage2 from "@/Pages/blogs/BlogPage2.vue";

const routes = [
  /*{ path: "/", component: LandingPage },*/
  /*{ path: "/", component: LandingPageNew},*/
  /*{ path: "/login", component: LoginPage },*/
  /*{ path: "/SignUp", component: SignUpPage },*/
  { path: "/dashboard", component: DashBoardPage,  meta: { requiresAuth: true } },
  { path: "/conversation", component: ConversationPage,  meta: { requiresAuth: false } },
  //{ path: "/scanImage", component: ScanImagePage },
  /*{ path: "/terms-and-conditions", component: GuidelinesPage },*/
  /*{ path: "/privacy-policy", component: PrivacyPolicyPage },*/
  /*{ path: "/license-agreement", component: LicenseAgreement },*/
  { path: "/reset-password", component: ResetPasswordPage },
  { path: "/profile-management", component: ProfileManagement,  meta: { requiresAuth: true } },
  { path: "/settings", component: SettingsPage,  meta: { requiresAuth: true }},
  /*{ path: "/chooseyourplan", component: PlanDetailsPage},*/
  { path: "/pricing", component: SubscriptionPage,  meta: { requiresAuth: true }},
  {path: "/logo", component: AppLogo},
  {path: "/headerImage", component: EmailHeaderLayOut},
  {path: "/error", component: ErrorPage},

  {
    path: "/",
    component: LandingPageNew,
    meta: {
      title: 'AI-Based Medical Scribe Software for Healthcare | MediNoteX ',
      description: 'Try MediNoteX AI, the AI-powered Medical Scribe Software for doctors. Simplify workflows with real-time transcription, prescription generation & reports. Book a demo now.'
    },
    children: [
      {
        path: '',
        component: HomeView
      },
      {
        path: "chooseyourplan",
        component: PlanDetailsPage,
        meta: {
          title: 'MediNoteX Pricing – Scalable Plans for Clinics and Hospitals',
          description: 'MediNoteX offers transparent, cost-effective pricing for its medical dictation software—ideal for individual practitioners and healthcare organizations. Book a demo now'
        }
      },
      {
        path: 'terms-and-conditions',
        component: TermsAndConditionsView,
        meta: {
         title: 'Terms & Conditions |  MediNoteX Medical Scribe Software',
         description: 'Review the terms and conditions for using MediNoteX, our AI-powered medical scribe software, including legal usage, service access, and user responsibilities'
       }
      },
      {
        path: "privacy-policy",
        component: PrivacyPolicyView,
        meta: {
          title: 'Privacy Policy | How MediNoteX Protects Your Data and Information',
          description: 'Learn how MediNoteX collects, uses, and protects your personal and clinical data. We are committed to safeguarding privacy for all users of our medical AI tools.'
        }
      },
      {
        path: "license-agreement",
        component: LicenseAgreementView,
        meta: {
         title: 'License Agreement | MediNoteX AI Medical Dictation Software Usage Rights',
         description: 'Read the MediNoteX license agreement to understand software usage rights, restrictions, and conditions for accessing our AI-based medical dictation software.'
       }
      },
      {
        path: "about-us",
        component: AboutUsView,
        meta: {
          title: ' Medical Scribe Solutions For Healthcare Professionals |MediNoteX',
          description: 'Transforming healthcare with AI medical scribe solutions that automate SOAP notes and streamline documentation for providers using MediNoteX. Start Free Trial Now'
        }
      },
      {
        path: "contact-us",
        component: ContactUsView,
        meta: {
            title: 'Contact MediNoteX – Connect with Our Medical Scribe Team Today',
            description: 'Have questions? Contact our team to learn how MediNoteX simplifies clinical documentation. Explore our medical transcribe platform features and start using now'
        }
      },
      {
        path: "request-demo",
          component: RequestDemoView,
          meta: {
              title: 'Book a Demo -AI Based Medical Transcribe Software',
              description: 'Schedule a demo with our AI medical trsncribe experts and explore the AI features of our MedinoteX platform. The best AI based medical transcriptions software for hospitals.'
          }
      },
      {
        path: "features",
        component: FeaturesPage,
        meta: {
          title: 'AI Based Medical Transcripion Software for Hospitals and Clinics',
          description: 'Empower your practice with AI Based Medical Transcription software for hospitals and clinics. MediNoteX enhances accuracy, reduces admin tasks & improves documentation.'
        }
      },
      {
        path: "resources",
          component: ResourcesView,
          meta: {
              title: 'Resources | AI in Medical Transcription & Healthcare Insights – MediNoteX',
              description: 'Explore expert blogs, guides, and case studies on AI-powered medical transcription. Stay updated with the latest healthcare documentation trends at MediNoteX.'
          }
      },
      {
        path: "resourcesblog1",
          component: BlogPage1,
          meta: {
              title: 'Transform Healthcare Documentation with AI Medical Scribe | MediNoteX',
              description: 'Boost efficiency and accuracy with AI medical scribe software. Automate documentation, reduce charting time, and enhance patient care. Book a demo with MediNoteX today!'
          }
      },
      {
        path: "resourcesblog2",
          component: BlogPage2,
          meta: {
              title: 'AI Medical Transcription Software | Revolutionize Healthcare | MediNoteX',
              description: 'Streamline clinical workflows with MediNoteX – the AI-powered medical transcription software for real-time, secure, and accurate SOAP notes. Book a demo now.'
          }
      }      
    ]
  },
   {
     path: '/',
     children: [
       {
         path: 'login',
         component: LoginPage,
         meta: {
          title: 'MediNoteX Login - Medical Dictation Software | Explore Now',
          description: 'AI-powered medical Dictation software that transcribes patient-doctor conversations in real time to streamline clinical documentation and integrate with your EHR systems.'
        }
       },
       {
         path: 'signup',
         component: SignUpPage,
         meta: {
          title: 'Sign Up for MediNoteX | Try AI Medical Scribe Software',
          description: 'Create your MediNoteX account to access AI-powered medical scribe tools that simplify clinical documentation for healthcare providers. Start your free trial today.'
        }
       }
     ]
   },
  {
    path: '/:pathMatch(.*)*',
    redirect: '/error'
  }
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
  scrollBehavior(to) {
    if (to.hash) {
      return {
        el: to.hash,
        behavior: 'smooth'
      };
    }
    return { top: 0 };
  }
});

// Global error handler
const handleRouteError = () => {
  router.push('/error');
};

//navigation guard
router.beforeEach((to, from, next) => {
  try {
    const isAuthenticated = document.cookie.includes('sessionToken');
    
    // Handle protected routes
    if (to.matched.some(record => record.meta.requiresAuth)) {
      if (!isAuthenticated) {
        sessionStorage.setItem('errorMessage', 'Access Denied: Please log in to continue. Your session has expired or you are not logged in.'); 
        next('/login');
        return;
      }
    }

    // Check if route exists
    if (!to.matched.length && to.path !== '/error') {
      sessionStorage.setItem('errorMessage', 'Page not found');
      next('/error');
      return;
    }

    document.title = to.meta.title;

    const descriptionTag = document.querySelector('meta[name="description"]');
    if (descriptionTag) {
      descriptionTag.setAttribute('content', to.meta.description);
    }

    next();
  } catch (error) {
    handleRouteError(error);
  }
});

// Handle navigation failures
router.onError((error) => {
  handleRouteError(error);
});
export default router;
