$(document).ready(function () {

    //Implement the functionality for an active link
    var currentUlr = window.location.pathname;

    $('.sidebar-link').each(function () {
        var linkUrl = $(this).attr('href');

        if (linkUrl == currentUlr) {
            $(this).addClass('active');
        }
    });

    // Function to calculate and update balance
    function updateBalance(totalPrice, amountPaid) {
        var balance = amountPaid - totalPrice;
        $('#balance').val(balance.toFixed(2));
    }

    //Display Time
    function updateTime() {
        const now = new Date();

        const hours = now.getHours();
        const minutes = now.getMinutes();
        const seconds = now.getSeconds();

        const formattedTime = formatTime(hours) + ':' + formatTime(minutes) + ':' + formatTime(seconds) + ' ' + getAMPM(hours);
        $('#current-time').text(formattedTime);

        const options = { weekday: 'short', year: 'numeric', month: 'short', day: 'numeric' };
        const formattedDate = now.toLocaleDateString(undefined, options);

        $('#current-date').text(formattedDate);
    }

    function formatTime(time) {
        return (time < 10) ? '0' + time : time;
    }

    function getAMPM(hours) {
        return (hours >= 12) ? 'PM' : 'AM';
    }

    updateTime();
    setInterval(updateTime, 1000);

    // Event listener for amount paid input
    $('#amount-paid').on('input', function () {
        var totalPrice = parseFloat($('#total-price').text());
        var amountPaid = parseFloat($(this).val() || 0);
        updateBalance(totalPrice, amountPaid);
    });

    // Event listener for payment method select
    $('#payment-method').on('change', function () {
        var totalPrice = parseFloat($('#total-price').text());
        var amountPaid = parseFloat($('#amount-paid').val() || 0);
        updateBalance(totalPrice, amountPaid);
    });

    $('#myForm').off().on('submit', function (event) {
        event.preventDefault();

        var totalPrice = parseFloat($('#total-price').text());
        var amountPaid = parseFloat($('#amount-paid').val() || 0);
        var paymentMethod = $('#payment-method').val();
        var balance = parseFloat($('#balance').val());

        if (totalPrice > 0 && balance >= 0) {
            var tableData = [];

            $('#cart-table tbody tr').each(function () {
                var productId = $(this).data('productId');
                var quantity = parseInt($(this).find('#quantity').text());
                var price = parseFloat($(this).find('#price').text());
                var name = $(this).find('#name').text();
                tableData.push({ Id: productId, Count: quantity, Price: price, Name: name }); 
            });

            var order = {
                Products: tableData, // Pass only the products with their IDs
                AmountPaid: amountPaid,
                TotalPrice: totalPrice,
                PaymentMethod: paymentMethod,
                Balance: balance
            }

            console.log(order);

            $.ajax({
                url: '/Order/ShowOrderReceipt/',
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(order),
                success: function (response) {
                    toastr.success('Order sent successfully!', 'Success', {
                        timeOut: 3000 // Set the duration of the notification (in milliseconds)
                    });

                    $('#cart-table tbody').empty();

                    $('#total-price').text('0.00');

                    $('#myForm')[0].reset();
                },
                error: function (xhr, status, error) {
                    console.error('Error: ', error);
                }
            });
        } else {
            alert("Order cannot be printed. Ensure total price is greater than 0 and balance is non-negative.")
        }

        console.log("sent");
    });



    // Function to populate product details in the table and calculate total price
    function populateCartTable(product) {
        var tableBody = $('#cart-table tbody');
        var existingRow = tableBody.find(`tr[data-product-id="${product.id}"]`);
        var totalPrice = 0;

        if (existingRow.length > 0) {
            // If the product already exists, increment the quantity
            var currentQuantity = parseInt(existingRow.find('td:eq(1)').text());
            var newQuantity = currentQuantity + product.count;
            existingRow.find('td:eq(1)').text(newQuantity);
        } else {
            // Append a new row with product details to the table
            var newRow = `<tr data-product-id="${product.id}">
                            <td id="name">${product.name}</td>
                            <td id="quantity">${product.count}</td>
                            <td id="price">${product.price}</td>
                            <td>
                                <span class="material-symbols-sharp remove-item">
                                    delete
                                </span>
                            </td>
                            <!-- Add more columns if needed -->
                        </tr>`;
            tableBody.append(newRow);
        }

        // Calculate total price by iterating through each row
        tableBody.find('tr').each(function () {
            var quantity = parseInt($(this).find('td:eq(1)').text());
            var price = parseFloat($(this).find('td:eq(2)').text());
            totalPrice += quantity * price;
        });

        // Update the element displaying the total price
        $('#total-price').text(totalPrice.toFixed(2)); // Assuming it's a text element displaying the total price
    }

    // Event listener for remove-item button
    $('#cart-table').on('click', '.remove-item', function () {
        var rowToRemove = $(this).closest('tr');
        var priceToRemove = parseFloat(rowToRemove.find('td:eq(2)').text());
        rowToRemove.remove();

        // Recalculate total price after removing the item
        var totalPrice = 0;
        $('#cart-table tbody').find('tr').each(function () {
            var quantity = parseInt($(this).find('td:eq(1)').text());
            var price = parseFloat($(this).find('td:eq(2)').text());
            totalPrice += quantity * price;
        });

        // Update the element displaying the total price
        $('#total-price').text(totalPrice.toFixed(2));
    });

    $('.menu-item').off().on('click', function () {
        var productId = $(this).data('productId');

        $.ajax({
            url: '/Product/GetProduct',
            method: 'GET',
            data: { id: productId },
            success: function (data) {
                // Call the function to populate the table with the product details
                populateCartTable(data);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });

    //Open Mobile Menu
    $('#menu-btn').on('click', function () {
        $('#aside').css('display', 'block');
    });

    //Close Menu
    $('#close-btn').on('click', function () {
        $('#aside').css('display', 'none');
    })
});