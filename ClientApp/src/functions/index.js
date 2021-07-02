export const objectToQueryString = filters => {
    let data = [];
    Object.keys(filters).forEach(key => {
        const value = filters[key];
        if (value !== null && value !== '') {
            data.push(`${key}=${value}`);
        }
    });
    if (data.length > 0) {
        return `?${data.join('&')}`;
    }
    return '';
}