import { NbMenuItem } from '@nebular/theme';

export const MENU_ITEMS: NbMenuItem[] = [
    {
        title: 'Admin Page',
        icon: 'home-outline',
        link: '/admin/index',
    },
    {
        title: 'Layout',
        icon: 'layout-outline',
        children: [
            {
                title: 'Stepper',
                link: '/pages/layout/stepper',
            },
            {
                title: 'List',
                link: '/pages/layout/list',
            },
            {
                title: 'Infinite List',
                link: '/pages/layout/infinite-list',
            },
            {
                title: 'Accordion',
                link: '/pages/layout/accordion',
            },
            {
                title: 'Tabs',
                pathMatch: 'prefix',
                link: '/pages/layout/tabs',
            },
        ],
    },
    {
        title: 'Auth',
        icon: 'lock-outline',
        children: [
            {
                title: 'Login',
                link: '/auth/login',
            },
            {
                title: 'Register',
                link: '/auth/register',
            },
            {
                title: 'Request Password',
                link: '/auth/request-password',
            },
            {
                title: 'Reset Password',
                link: '/auth/reset-password',
            },
        ],
    },
];
