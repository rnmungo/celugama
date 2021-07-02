import React from 'react';
import { BrowserRouter as Router, Switch } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';

// Pages
import MeliPage from './pages/MeliPage';
import LoggedInPage from './pages/LoggedInPage';
import OrdersPage from './pages/OrdersPage';
import HomePage from './pages/HomePage';
import NotFoundPage from './pages/NotFoundPage';
import LoginPage from './pages/LoginPage';
import ChangePasswordPage from './pages/ChangePasswordPage';

// Middlewares
import AuthRoute from './middlewares/AuthRoute';
import LoginRoute from './middlewares/LoginRoute';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');

const App = () => (
    <Router basename={baseUrl}>
        <SnackbarProvider maxSnack={3}>
            <Switch>
                <AuthRoute path='/' exact component={HomePage} />
                <AuthRoute path='/token' exact component={MeliPage} />
                <AuthRoute path='/success' component={LoggedInPage} />
                <AuthRoute path='/orders' exact component={OrdersPage} />
                <AuthRoute path='/change-password' exact component={ChangePasswordPage} />
                <LoginRoute path='/login' exact component={LoginPage} />
                <AuthRoute path='*' exact component={NotFoundPage} />
            </Switch>
        </SnackbarProvider>
    </Router>
);

export default App;