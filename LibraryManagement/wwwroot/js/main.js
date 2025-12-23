(function ($) {

    "use strict";

    $(document).ready(function () {

        // Initialize base Select2 dropdowns

        function initializeSelect2(parentElement) {

            $(parentElement).find('.searchable-select').each(function () {

                var $select = $(this);

                $select.select2({

                    theme: 'bootstrap-5',

                    width: '100%',

                    dropdownParent: $(parentElement).is('body') ? $('body') : $(parentElement),

                    sorter: function (data) {

                        return data.sort(function (a, b) {

                            return a.text.localeCompare(b.text, undefined, { sensitivity: 'base' });

                        });

                    },

                    templateSelection: function (data) {

                        if (!data.id) { return data.text; }

                        return $('<span>').text(data.text).addClass('text-truncate');

                    }

                }).on('select2:opening', function (e) {

                    $select.data('previousSelection', $select.val());

                }).on('select2:select', function (e) {

                    $select.val(e.params.data.id).trigger('change');

                }).on('select2:closing', function (e) {

                    if (!$select.val()) {

                        $select.val($select.data('previousSelection')).trigger('change');

                    }

                });

            });

        }

        $(document).on('select2:open', function () {

            setTimeout(() => {

                let searchField = document.querySelector('.select2-container--open .select2-search__field');

                if (searchField) {

                    searchField.focus();

                    searchField.click(); // Ensures cursor appears inside input

                }

            }, 50);

        });

        // Initialize Select2 for outside selects

        initializeSelect2('body');

        // Initialize Select2 for all modals

        $('.modal').each(function () {

            var modalId = '#' + $(this).attr('id');

            initializeSelect2(modalId);

            $(this).on('shown.bs.modal', function () {

                initializeSelect2(modalId);

            });

        });

        // Handle dropdown menu clicks

        $('.dropdown-menu').on('click', function (e) {

            e.stopPropagation();

        });

        // Handle datepicker triggers

        $('.trigger-datepicker').on('click', function () {

            $($(this).data('target')).datepicker('show');

        });

    });

})(jQuery);
