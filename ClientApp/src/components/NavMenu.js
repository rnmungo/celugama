import React, { useState } from 'react';

// Components
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import IconButton from '@material-ui/core/IconButton';
import SwipeableDrawer from '@material-ui/core/SwipeableDrawer';
import List from '@material-ui/core/List';
import Divider from '@material-ui/core/Divider';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import { Link } from 'react-router-dom';
import HomeIcon from '@material-ui/icons/Home';
import MenuIcon from '@material-ui/icons/Menu';
import ListAltIcon from '@material-ui/icons/ListAlt';
import ExitToAppIcon from '@material-ui/icons/ExitToApp';
import LockIcon from '@material-ui/icons/Lock';
import HowToRegIcon from '@material-ui/icons/HowToReg';

// Assets
import logo from '../assets/img/LogoMercadoLibre.png';

// Styles
import { makeStyles } from '@material-ui/core/styles';
import yellow from '@material-ui/core/colors/yellow';

const useStyles = makeStyles(theme => ({
    title: {
        flexGrow: 1,
    },
    img: {
        height: 'auto',
        width: 50,
        marginLeft: theme.spacing(1)
    },
    appBar: {
        backgroundColor: yellow[400]
    },
    list: {
        width: 250,
    }
}));

const NavMenu = ({ history }) => {

    const classes = useStyles();
    const [open, setOpen] = useState(false);

    const toggleDrawer = evt => {
        if (evt && evt.type === 'keydown' && (evt.key === 'Tab' || evt.key === 'Shift')) {
            return;
        }

        setOpen(state => setOpen(!state));
    };

    const logout = () => {
        localStorage.removeItem('celugama-token');
        localStorage.removeItem('celugama-username');
        localStorage.removeItem('celugama-expires');
        history.push('/');
    }

    return (
        <header>
            <AppBar position="static" className={classes.appBar}>
                <Toolbar>
                    <Typography variant="h6" className={classes.title}>
                        <IconButton onClick={() => toggleDrawer(true)} color="inherit">
                            <MenuIcon />
                        </IconButton>
                    </Typography>
                    <img className={classes.img} src={logo} alt={logo} />
                </Toolbar>
            </AppBar>
            <SwipeableDrawer
                anchor="left"
                open={open}
                onClose={() => toggleDrawer(false)}
                onOpen={() => toggleDrawer(true)}
            >
                <div
                    className={classes.list}
                    role="presentation"
                    onClick={() => toggleDrawer(false)}
                    onKeyDown={() => toggleDrawer(false)}
                >
                    <List>
                        <ListItem button component={Link} to="/">
                            <ListItemIcon><HomeIcon /></ListItemIcon>
                            <ListItemText primary="Inicio" />
                        </ListItem>
                        <ListItem button component={Link} to="/orders">
                            <ListItemIcon><ListAltIcon /></ListItemIcon>
                            <ListItemText primary="Pedidos" />
                        </ListItem>
                        <ListItem button component={Link} to="/token">
                            <ListItemIcon><HowToRegIcon /></ListItemIcon>
                            <ListItemText primary="Autenticarse con MercadoLibre" />
                        </ListItem>
                    </List>
                    <Divider />
                    <List>
                        <ListItem button component={Link} to="/change-password">
                            <ListItemIcon><LockIcon /></ListItemIcon>
                            <ListItemText primary="Cambiar Contrase&ntilde;a" />
                        </ListItem>
                        <ListItem button onClick={logout}>
                            <ListItemIcon><ExitToAppIcon /></ListItemIcon>
                            <ListItemText primary="Cerrar Sesi&oacute;n" />
                        </ListItem>
                    </List>
                </div>
            </SwipeableDrawer>
        </header>
    );
}

export default NavMenu;
