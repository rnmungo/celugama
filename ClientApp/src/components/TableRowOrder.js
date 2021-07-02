import React, { useState, useEffect, forwardRef, Fragment } from 'react';
import { useSnackbar } from 'notistack';

// Service
import { getOrder } from '../services/orderService';

// Components
import TableRow from '@material-ui/core/TableRow';
import TableCell from '@material-ui/core/TableCell';
import Checkbox from '@material-ui/core/Checkbox';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import AppBar from '@material-ui/core/AppBar';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import Toolbar from '@material-ui/core/Toolbar';
import IconButton from '@material-ui/core/IconButton';
import Typography from '@material-ui/core/Typography';
import Divider from '@material-ui/core/Divider';
import Slide from '@material-ui/core/Slide';
import CloseIcon from '@material-ui/icons/Close';

import PublicationListItem from './PublicationListItem';

// Styles
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
    appBar: {
        position: 'relative',
    },
    formControl: {
        margin: theme.spacing(1)
    },
    inline: {
        display: 'inline'
    }
}));

const Transition = forwardRef(function Transition(props, ref) {
    return <Slide direction="up" ref={ref} {...props} />;
});

const TableRowOrder = ({ order, date, selected, disabled, onCheckHandler, destiny }) => {
    const [product, setProduct] = useState(false);
    const [address, setAddress] = useState(false);
    const [items, setItems] = useState([]);

    const classes = useStyles();
    const { enqueueSnackbar } = useSnackbar();

    const sendNotification = (variant, message) => {
        enqueueSnackbar(message, { variant });
    };

    useEffect(() => {
        console.log(order);
        getOrder(order.orderIDML).then((data) => {
            if (data !== null) {
                setItems(data);
            } else {
                sendNotification('info', 'No se pudieron cargar los artículos');
            }
        }).catch((e) => {
            sendNotification('error', e.message);
        });
    }, []);

    return (
        <TableRow hover role="checkbox" tabIndex={-1} key={order.id}>
            {order.isPrintable && (
                <TableCell padding="checkbox">
                    <Checkbox checked={selected} disabled={disabled} onChange={() => onCheckHandler(order.orderIDML)} />
                </TableCell>
            )}
            <TableCell align="left">{date}</TableCell>
            <TableCell align="left">{order.orderIDML}</TableCell>
            <TableCell align="left">{order.shippingId}</TableCell>
            <TableCell align="left">{order.packId}</TableCell>
            <TableCell align="left">{`${order.lastName}, ${order.name}`}</TableCell>
            <TableCell align="left">{order.items}</TableCell>
            <TableCell align="center">
                <Button
                    color="secondary"
                    variant="contained"
                    className={classes.formControl}
                    onClick={() => setProduct(true)}>
                    VER PRODUCTO
                </Button>
                <Dialog fullScreen open={product} onClose={() => setProduct(false)} TransitionComponent={Transition}>
                    <AppBar className={classes.appBar}>
                        <Toolbar>
                            <IconButton edge="start" color="inherit" onClick={() => setProduct(false)} aria-label="close">
                                <CloseIcon />
                            </IconButton>
                        </Toolbar>
                    </AppBar>
                    <List>
                        {items && items.map((item, index) => {
                            return <PublicationListItem key={index} {...item} />;
                        })}
                    </List>
                </Dialog>
                {destiny && (
                    <Fragment>
                        <Button
                            color="secondary"
                            variant="contained"
                            className={classes.formControl}
                            onClick={() => setAddress(true)}>
                            VER DIRECCIÓN
                        </Button>
                        <Dialog fullScreen open={address} onClose={() => setAddress(false)} TransitionComponent={Transition}>
                            <AppBar className={classes.appBar}>
                                <Toolbar>
                                    <IconButton edge="start" color="inherit" onClick={() => setAddress(false)} aria-label="close">
                                        <CloseIcon />
                                    </IconButton>
                                </Toolbar>
                            </AppBar>
                            <List>
                                <ListItem button>
                                    <ListItemText primary={<b>Dirección</b>} secondary={
                                        <Fragment>
                                            <Typography
                                                component="span"
                                                variant="body2"
                                                className={classes.inline}
                                                color="textPrimary">
                                                {order.receiverAddress}
                                            </Typography>
                                            {` - ${order.receiverCity}, ${order.receiverState}, ${order.receiverCountry}, C.P. ${order.receiverZipCode}`}
                                        </Fragment>
                                    } />
                                </ListItem>
                                <Divider />
                                <ListItem button>
                                    <ListItemText primary={<b>Contacto</b>} secondary={
                                        <Fragment>
                                            <Typography
                                                component="span"
                                                variant="body2"
                                                className={classes.inline}
                                                color="textPrimary">
                                                {order.receiverName}
                                            </Typography>
                                            {` - Tel. ${order.receiverPhone}`}
                                        </Fragment>
                                    } />
                                </ListItem>
                            </List>
                        </Dialog>
                    </Fragment>
                )}
            </TableCell>
        </TableRow>
    );
}

export default TableRowOrder;