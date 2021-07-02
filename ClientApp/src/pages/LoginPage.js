import React, { useState, useEffect, useRef } from 'react';
import { useSnackbar } from 'notistack';

// Services
import { tryLogin } from '../services/loginService';

// Components
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import TextField from '@material-ui/core/TextField';
import Link from '@material-ui/core/Link';
import Paper from '@material-ui/core/Paper';
import Box from '@material-ui/core/Box';
import Grid from '@material-ui/core/Grid';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';

import CircularLoadingBackdrop from '../components/animations/loading/CircularLoadingBackdrop';

// Styles
import { createMuiTheme, makeStyles, responsiveFontSizes } from '@material-ui/core/styles';
import { ThemeProvider } from '@material-ui/styles';
import yellow from '@material-ui/core/colors/yellow';
import blue from '@material-ui/core/colors/blue';

// Assets
import Warehouse from '../assets/img/Warehouse.jpg';

const theme = responsiveFontSizes(createMuiTheme({
    palette: {
        primary: yellow,
        secondary: blue,
    },
}));

const useStyles = makeStyles(theme => ({
    root: {
        height: '100vh',
    },
    image: {
        backgroundImage: `url(${Warehouse})`,
        backgroundRepeat: 'no-repeat',
        backgroundColor:
            theme.palette.type === 'dark' ? theme.palette.grey[900] : theme.palette.grey[50],
        backgroundSize: 'cover',
        backgroundPosition: 'center',
    },
    paper: {
        margin: theme.spacing(8, 4),
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    },
    avatar: {
        margin: theme.spacing(1),
        color: '#000',
        backgroundColor: yellow['A400'],
    },
    form: {
        width: '100%', // Fix IE 11 issue.
        marginTop: theme.spacing(1),
    },
    submit: {
        margin: theme.spacing(3, 0, 2),
    },
}));

const Copyright = () => {
    return (
        <Typography variant="body2" color="textSecondary" align="center">
            {'Copyright © '}
            <Link color="inherit" href="https://www.proyectomas.net/">
                ProyectoMas
            </Link>{' '}
            {new Date().getFullYear()}
        </Typography>
    );
}

const LoginPage = (props) => {

    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [loading, setLoading] = useState(false);

    const classes = useStyles();

    const { enqueueSnackbar } = useSnackbar();

    const sendNotification = (variant, message) => {
        enqueueSnackbar(message, { variant });
    }

    const timer = useRef();

    useEffect(() => {
        return () => {
            clearTimeout(timer.current)
        }
    }, []);

    const login = async (evt) => {
        evt.preventDefault();
        setLoading(true);
        if (username === '' || password === '') {
            sendNotification('warning', 'El usuario y contraseña son requeridos');
            setLoading(false);
            return;
        }

        timer.current = setTimeout(async () => {
            try {
                const result = await tryLogin(username, password);
                localStorage.setItem('celugama-token', result.token);
                localStorage.setItem('celugama-username', result.username);
                localStorage.setItem('celugama-expires', result.expires);
            } catch (e) {
                sendNotification('error', e.message);
            }
            setLoading(false);
        }, 500);
    };

    useEffect(() => {
        const accessToken = localStorage.getItem('celugama-token');
        if (accessToken) {
            props.history.push('/');
        }
    });

    return (
        <ThemeProvider theme={theme}>
            <CircularLoadingBackdrop show={loading} label="Iniciando sesión" />
            <Grid container component="main" className={classes.root}>
                <CssBaseline />
                <Grid item xs={false} sm={4} md={7} className={classes.image} />
                <Grid item xs={12} sm={8} md={5} component={Paper} elevation={6} square>
                    <div className={classes.paper}>
                        <Avatar className={classes.avatar}>
                            <LockOutlinedIcon />
                        </Avatar>
                        <Typography component="h1" variant="h5">
                            Iniciar Sesión
                        </Typography>
                        <form onSubmit={login} className={classes.form}>
                            <TextField
                                margin="normal"
                                color="primary"
                                required
                                fullWidth
                                id="username"
                                label="Usuario"
                                name="username"
                                autoComplete="username"
                                autoFocus
                                value={username}
                                onChange={(evt) => setUsername(evt.target.value)}
                            />
                            <TextField
                                margin="normal"
                                color="primary"
                                required
                                fullWidth
                                name="password"
                                label="Contraseña"
                                type="password"
                                id="password"
                                autoComplete="current-password"
                                value={password}
                                onChange={(evt) => setPassword(evt.target.value)}
                            />
                            <Button
                                fullWidth
                                type="submit"
                                variant="contained"
                                color="primary"
                                className={classes.submit}
                            >
                                Iniciar Sesión
                            </Button>
                            <Box mt={5}>
                                <Copyright />
                            </Box>
                        </form>
                    </div>
                </Grid>
            </Grid>
        </ThemeProvider>
    );
};

export default LoginPage;