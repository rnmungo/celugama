import React from 'react';
import Grid from '@material-ui/core/Grid';
import NavMenu from './NavMenu';

import { createMuiTheme, makeStyles, responsiveFontSizes } from '@material-ui/core/styles';
import { ThemeProvider } from '@material-ui/styles';
import yellow from '@material-ui/core/colors/yellow';
import blue from '@material-ui/core/colors/blue';

const theme = responsiveFontSizes(createMuiTheme({
    palette: {
        primary: yellow,
        secondary: blue,
    },
}));

const useStyles = makeStyles(theme => ({
    grid: {
        marginTop: theme.spacing(3)
    }
}));

const Layout = ({ children, history }) => {

    const classes = useStyles();

    return (
        <ThemeProvider theme={theme}>
            <NavMenu history={history} />
            <Grid container justify="center" className={classes.grid}>
                {children}
            </Grid>
        </ThemeProvider>
    );
};

export default Layout;