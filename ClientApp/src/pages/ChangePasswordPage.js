import React, { useState } from 'react';
import { useSnackbar } from 'notistack';

// Services
import { changePassword } from '../services/loginService';

// Components
import TextField from '@material-ui/core/TextField';
import Grid from '@material-ui/core/Grid';
import Button from '@material-ui/core/Button';
import Typography from '@material-ui/core/Typography';
import InputAdornment from '@material-ui/core/InputAdornment';
import Layout from '../components/Layout';
import PasswordSecureIcon from '../components/PasswordSecureIcon';
import PasswordMatchIcon from '../components/PasswordMatchIcon';
import CircularLoadingBackdrop from '../components/animations/loading/CircularLoadingBackdrop';

// Styles
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
    form: {
        width: '100%', // Fix IE 11 issue.
        marginTop: theme.spacing(1),
    },
    submit: {
        margin: theme.spacing(3, 0, 2),
    },
    paper: {
        margin: theme.spacing(3),
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    },
}));

const ChangePasswordPage = props => {

    const [newPassword, setNewPassword] = useState('');
    const [repeat, setRepeat] = useState('');
    const [loading, setLoading] = useState(false);

    const classes = useStyles();

    const { enqueueSnackbar } = useSnackbar();

    const sendNotification = (variant, message) => {
        enqueueSnackbar(message, { variant });
    }

    const handleOnSubmit = async (evt) => {
        evt.preventDefault();
        change();
    };

    const change = async () => {
        setLoading(true);
        try {
            await changePassword(newPassword);
            setLoading(false);
            setNewPassword('');
            setRepeat('');
            sendNotification('success', 'Su contraseña fue modificada');
        } catch (e) {
            setLoading(false);
            sendNotification('error', e.message);
        }
    };

    return (
        <Layout {...props}>
            <CircularLoadingBackdrop show={loading} label="Cambiando contraseña" />
            <Grid container justify="center">
                <Grid item xs={12} sm={8} md={6} lg={4}>
                    <div className={classes.paper}>
                        <form onSubmit={handleOnSubmit} className={classes.form}>
                            <Typography variant="h5" align="center" gutterBottom>
                                Cambio de contraseña
                            </Typography>
                            <TextField
                                margin="normal"
                                color="primary"
                                required
                                fullWidth
                                id="password"
                                label="Nueva contraseña"
                                type="password"
                                name="password"
                                autoComplete="password"
                                helperText="Para ser una contraseña segura, debe contener al menos una letra mayúscula, una letra minúscula, caracteres numéricos y un caracter especial (#, $, &, etc.)"
                                InputProps={{
                                    startAdornment: <InputAdornment position="start"><PasswordSecureIcon password={newPassword} /></InputAdornment>,
                                }}
                                value={newPassword}
                                onChange={(evt) => setNewPassword(evt.target.value)}
                            />
                            <TextField
                                margin="normal"
                                color="primary"
                                required
                                fullWidth
                                id="repeat"
                                label="Repetir contraseña"
                                type="password"
                                name="repeat"
                                autoComplete="password"
                                InputProps={{
                                    startAdornment: <InputAdornment position="start"><PasswordMatchIcon password={newPassword} toCompare={repeat} /></InputAdornment>,
                                }}
                                value={repeat}
                                onChange={(evt) => setRepeat(evt.target.value)}
                            />
                            <Button
                                fullWidth
                                type="submit"
                                variant="contained"
                                color="primary"
                                className={classes.submit}
                            >
                                Guardar cambios
                            </Button>
                        </form>
                    </div>
                </Grid>
            </Grid>
        </Layout>
    )
};

export default ChangePasswordPage;